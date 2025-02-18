using SterreWebApi.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add authorization
builder.Services.AddAuthorization();

// Add OpenAPI support for Swagger (if needed)
builder.Services.AddOpenApi();

// Set up configuration to use user secrets (in development) and environment variables
builder.Configuration
    .AddEnvironmentVariables() // Add environment variables (for production and other environments)
    .AddUserSecrets<Program>(); // Add user secrets (in development)

var connectionString = builder.Configuration["SqlConnectionString"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is missing or invalid.");
}

//Console.WriteLine(connectionString);

// Add repositories with the connection string
builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(provider => new Environment2DRepository(connectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(provider => new Object2DRepository(connectionString));

var app = builder.Build();

// Enable OpenAPI (Swagger) for development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Define a basic route for testing
app.MapGet("/", () => "Hello world, the API is up");

// Use HTTPS redirection and authorization middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
