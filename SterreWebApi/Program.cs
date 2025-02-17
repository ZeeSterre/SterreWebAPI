using SterreWebApi.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Ensure authorization services are registered
builder.Services.AddAuthorization();  // <-- This line adds the required authorization services

builder.Services.AddOpenApi();

// Add any other required services, like database context, repositories, etc.
var connectionString = builder.Configuration["SqlConnectionString"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is missing or invalid.");
}

Console.WriteLine($"Connection String: {connectionString}"); // Debugging purpose

// Register repositories with dependency injection
builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(provider => new Environment2DRepository(connectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(provider => new Object2DRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Hello world, the API is up");

app.UseHttpsRedirection();

// Make sure to add the authorization middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
