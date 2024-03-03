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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddAuthentication(options => { 
   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //"Bearer"
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}) .AddJwtBearer(options =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Add Serilog Request Logging
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
