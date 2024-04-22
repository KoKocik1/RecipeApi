using RecipeApi.Database;
using Microsoft.EntityFrameworkCore;
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
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using RecipeApi.Settings;
using RecipeApi.Tools;

var builder = WebApplication.CreateBuilder(args);

// Nlog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
//builder.Host.UseNLog();

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
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey ?? string.Empty)),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero,
    };
    // cfg.RequireHttpsMetadata = false;
    // cfg.SaveToken = true;
    // cfg.TokenValidationParameters = new TokenValidationParameters
    // {
    //     ValidIssuer = authenticationSettings.JwtIssuer,
    //     ValidAudience = authenticationSettings.JwtIssuer,
    //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    // };
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("SameAuthor", policy => policy.AddRequirements(new SameAuthorRequirement()));
});

//Authorization
builder.Services.AddScoped<IAuthorizationHandler, SameAuthorRequirementHandler>();


builder.Services.AddDbContext<RecipeDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;

})
    .AddEntityFrameworkStores<RecipeDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
    options.Cookie.Name = "Identity.Cookie";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SMTP"));

// Mapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

builder.Services.AddControllers().AddFluentValidation();
//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddAuthorizationBuilder(); //TODO: new



// Seeder
builder.Services.AddScoped<RecipeSeeder>();

builder.Services.AddScoped<IIngrededientService, IngredientService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRecipeInstructionService, RecipeInstructionService>();
builder.Services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();
builder.Services.AddScoped<IUnitIngredientService, UnitIngredientService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddScoped<ITokenHelper, TokenHelper>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();


// builder.Services
//     .AddIdentityApiEndpoints<User>()
//     .AddEntityFrameworkStores<RecipeDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>

        policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});



var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RecipeSeeder>();


app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");
seeder.Seed();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

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

