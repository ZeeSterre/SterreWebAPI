using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using SterreWebApi.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SqlConnectionString");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is missing");
}

// Register repositories with dependency injection
builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(provider => new Environment2DRepository(connectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(provider => new Object2DRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();