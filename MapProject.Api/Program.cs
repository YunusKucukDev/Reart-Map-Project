using MapProject.Api.Data;
using MapProject.Api.Entities;
using MapProject.Api.Services.CategoryService;
using MapProject.Api.Services.ContactService;
using MapProject.Api.Services.IdentityService;
using MapProject.Api.Services.MapIdentityDescriptionService;
using MapProject.Api.Services.UserInformationService;
using MapProject.Api.Services.VisitorLogService;
using MapProject.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);


var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();


builder.Services.AddSingleton<MongoDB.Driver.IMongoClient>(sp =>
    new MongoDB.Driver.MongoClient(databaseSettings.ConnectionString));


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

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserInformationService, UserInformationService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IMapIdentityDescriptionService, MapIdentityDescriptionService>();
builder.Services.AddScoped<IVisitorLogService, VisitorLogService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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
