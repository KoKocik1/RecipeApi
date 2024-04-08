using RecipeApi.Database;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using RecipeApi.Seeder;
using RecipeApi.IService;
using RecipeApi.Service;
using RecipeApi.Middleware;
using RecipeApi.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RecipeApi.Models;
using RecipeApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Nlog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// authentication
var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});
// builder.Services.AddAuthorization(option =>
// {
//     option.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
//     option.AddPolicy("atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
//     option.AddPolicy("CreatedAtLeast1Recipe", builder => builder.AddRequirements(new MinimumOneRestaurant(1)));
// });

//Authorization
// builder.Services.AddScoped<IAuthorizationHandler, MinimumTwoRestaurantHandler>();
// builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
// builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();



builder.Services.AddDbContext<RecipeDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Mapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

// Seeder
builder.Services.AddScoped<RecipeSeeder>();

builder.Services.AddScoped<IIngrededientService, IngredientService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>

        policyBuilder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(builder.Configuration["AllowedOrigins"])
    );
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RecipeSeeder>();


app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");

seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

