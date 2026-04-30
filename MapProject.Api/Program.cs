using MapProject.Api.Data;
using MapProject.Api.Entities;
using MapProject.Api.Services.CategoryService;
using MapProject.Api.Services.ContactService;
using MapProject.Api.Services.CourselService;
using MapProject.Api.Services.IdentityService;
using MapProject.Api.Services.MapIdentityDescriptionService;
using MapProject.Api.Services.MapViewerService;
using MapProject.Api.Services.UserInformationService;
using MapProject.Api.Services.VideoService;
using MapProject.Api.Services.VisitorLogService;
using MapProject.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

// .env dosyasını yükle
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// --- Konfigürasyonlar ---
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

builder.Services.AddSingleton<MongoDB.Driver.IMongoClient>(sp =>
    new MongoDB.Driver.MongoClient(databaseSettings.ConnectionString));

// --- CORS AYARI 
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowWebUI", policy =>
    {
        
        policy.WithOrigins("https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// MongoDB Identity
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddMongoDbStores<AppUser, AppRole, Guid>(
        databaseSettings.ConnectionString,
        databaseSettings.DatabaseName
    );

// JWT Authentication
var secretKey = builder.Configuration["JWTSecurity:SecretKey"]!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Servisler
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserInformationService, UserInformationService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IMapIdentityDescriptionService, MapIdentityDescriptionService>();
builder.Services.AddScoped<IVisitorLogService, VisitorLogService>();
builder.Services.AddScoped<ICoureselService, CoureselService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IMapViewerService, MapViewerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Ortam Ayarları ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// CORS Politikasını Uygula
app.UseCors("AllowWebUI");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Database Seeding
try
{
    await SeedDatabase.Initialize(app);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "SeedDatabase.Initialize başarısız oldu: {Message}", ex.Message);
}

app.Run();