using RecipeApi.Database;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using RecipeApi.Seeder;
using RecipeApi.IService;
using RecipeApi.Service;
using RecipeApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Nlog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();


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

