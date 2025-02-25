using Microsoft.AspNetCore.Identity;
using SterreWebApi.Repositorys;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connectionstring
var connectionString = builder.Configuration["SqlConnectionString"]
    ?? throw new InvalidOperationException("Connection string is missing");
builder.Services.AddSingleton(connectionString!);

builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();
builder.Services.AddSingleton<IUserInfoRepository>(new UserInfoRepository(connectionString));
builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(provider => new Environment2DRepository(connectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(provider => new Object2DRepository(connectionString));
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
        options.Password.RequiredLength = 10;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddDapperStores(options =>
    {
        options.ConnectionString = connectionString;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserEntity", policy =>
        policy.RequireClaim("entity:user", "true"));

    options.AddPolicy("AdminEntity", policy =>
        policy.RequireClaim("entity:admin", "true"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{ 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/account/register", async (RegisterRequest request, UserManager<AppUser> userManager) =>
{
    if (string.IsNullOrEmpty(request.Password) || request.Password.Length < 10)
    {
        return Results.BadRequest("Password does not meet the requirements.");
    }
    var existingUser = await userManager.FindByNameAsync(request.UserName);
    if (existingUser != null)
    {
        return Results.BadRequest("Username is already in use.");
    }

    var user = new AppUser
    {
        UserName = request.UserName
    };

    var result = await userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded)
    { 
        return Results.BadRequest(result.Errors);
    }
    var claimResult = await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("entity:user", "true"));

    if (!claimResult.Succeeded)
    {
        return Results.BadRequest(claimResult.Errors);
    }

    return Results.Ok("Registration successful!");
});

app.MapPost("/account/login", async (LoginRequest request, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) =>
{
    if (signInManager.Context.User?.Identity?.IsAuthenticated == true)
    { 
        return Results.BadRequest("User is already logged in.");
    }

    if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
    {
        return Results.BadRequest("Username and password must be provided.");
    }

    var user = await userManager.FindByNameAsync(request.UserName);
    if (user == null)
    {
        return Results.BadRequest("Invalid username or password.");
    }

    var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
    if (!result.Succeeded)
    {
        return Results.BadRequest("Invalid username or password.");
    }
    return Results.Ok("Login successful!");
});

app.MapPost("/account/logout", async (SignInManager<AppUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "API is up");
app.Run();



