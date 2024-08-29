using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolarWatchORM.Data;
using SolarWatchORM.Data.CityData;
using SolarWatchORM.Service.CityDataProvider;
using SolarWatchORM.Service.CityRepo;
using SolarWatchORM.Service.SunDataProvider;
using SolarWatchORM.Service.SunRepo;
using SolarWatchORM.Service.UserService;
using System;
using System.Text;
using SolarWatchORM.Configurations;
using SolarWatchORM.Seeding;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddUserSecrets<Program>();


builder.Services.AddSingleton<ICityDataProvider, CityDataProvider>();
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddSingleton<ISunDataProvider, SunDataProvider>();
builder.Services.AddScoped<ISunRepo, SunRepo>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SolarWatchContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<IdentityContext>(options =>options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserService>(); 


var jwtSettings = builder.Configuration.GetSection("Jwt");
var issuerSigningKey = builder.Configuration["Jwt:IssuerSigningKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwt"];
            return Task.CompletedTask;
        }
    };
})
.AddCookie(options =>
 {
     options.Cookie.HttpOnly = true;
     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
     options.Cookie.SameSite = SameSiteMode.None;
     options.Cookie.Name = "jwt";
     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
 });

builder.Services.AddScoped<JwtTokenHelper>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
    var userContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

    dbContext.Database.Migrate();

    userContext.Database.Migrate();

    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }


