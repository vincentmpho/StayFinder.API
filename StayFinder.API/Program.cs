using Microsoft.EntityFrameworkCore;
using Serilog;
using StayFinder.API.Configaration;
using StayFinder.API.Data;
using StayFinder.API.Repository;
using StayFinder.API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using StayFinder.API.Models;

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
                .AddEntityFrameworkStores<StayFinderDbContext>();


//register autoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

//Register Repostiory
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
