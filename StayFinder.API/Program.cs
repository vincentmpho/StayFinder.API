using Microsoft.EntityFrameworkCore;
using Serilog;
using StayFinder.API.Configaration;
using StayFinder.API.Data;
using StayFinder.API.Repository;
using StayFinder.API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using StayFinder.API.Models;
using StayFinder.API.Services.Interface;
using StayFinder.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Stay Finder API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer' 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                           Type =ReferenceType.SecurityScheme,
                           Id = "Bearer"
                },
                Scheme = "Oauth2",
                       Name = "Bearer",
                       In = ParameterLocation.Header
            },
         new List<string>()
        }
    });
});

//inject Cors to llow another domains 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyHeader()
              .AllowAnyOrigin()
              .AllowAnyMethod());
});

//register User Identity Core
builder.Services.AddIdentityCore<ApiUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("StayFinderAPI")
                .AddEntityFrameworkStores<StayFinderDbContext>()
                .AddDefaultTokenProviders();


//register autoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

// inject  Repostiory
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();

//inject Services
builder.Services.AddScoped<IAuthManager, AuthManager>();

//inject JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //"Bearer"
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyWithAtLeast256BitsOrMore"))



    };

});



//register DB connection
var connectionString = builder.Configuration.GetConnectionString("StayFinderDbConnectionString");
builder.Services.AddDbContext<StayFinderDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//inject Serilog
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo
.Console()
.ReadFrom.Configuration(ctx.Configuration));

//inject Helath Service Checks
builder.Services.AddHealthChecks()
    .AddCheck<CustomHealthCheck>("Custom Health Check", failureStatus: HealthStatus.Degraded,
    tags: new[] {"custom" }
    
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Add Health check Middleware
app.MapHealthChecks("/healthcheck", new HealthCheckOptions
{ 
    Predicate = healthcheck =>  healthcheck.Tags.Contains("custom")

});

app.MapHealthChecks("/healthcheck");

//Add Serilog Request Logging
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        /* custom checks. Logic.... etc.etc. */

        if (isHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy("All system are looking good"));
        }
        return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "System unhealthy"));
    }
}