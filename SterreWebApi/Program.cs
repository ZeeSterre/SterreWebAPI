using SterreWebApi.Repositorys;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("SecretSql.json", optional: false, reloadOnChange: true);

var connectionString = builder.Configuration["SqlConnectionString"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is missing or invalid.");
}

builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(provider => new Environment2DRepository(connectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(provider => new Object2DRepository(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Hello world, the API is up");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
