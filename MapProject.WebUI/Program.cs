using MapProject.WebUI.Hubs;
using MapProject.WebUI.Services.EmailService;
using MapProject.WebUI.Services.CategoryService;
using MapProject.WebUI.Services.ContactService;
using MapProject.WebUI.Services.IdentityService;
using MapProject.WebUI.Services.MapIdentityDescriptionService;
using MapProject.WebUI.Services.UserInformationService;
using MapProject.WebUI.Services.VisitorLogService;
using MapProject.WebUI.Services.VisitorService;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Temel Servisler
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<VisitorTracker>();

// 2. Email Ayarlarý
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// 3. Kimlik Doðrulama (Cookie) Ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "MapProject.Cookie";
    });

// 4. API Baðlantý Ayarlarý (HttpClient Factory)
// appsettings.json dosyanýzdaki "ApiSettings:BaseUrl" deðerini okur.
var apiUri = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);

builder.Services.AddHttpClient<ICategoryService, CategoryService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IContactService, ContactService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IMapIdentityDescriptionService, MapIdentityDescriptionService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IUserInformationService, UserInformationService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IIdentityService, IdentityService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IVisitorLogService, VisitorLogService>(opt => opt.BaseAddress = apiUri);

// 5. Uygulama Oluþturma
var app = builder.Build();

// 6. Middleware (Ara Yazýlým) Yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Önce Authentication (Kimlik Doðrulama), Sonra Authorization (Yetkilendirme)
app.UseAuthentication();
app.UseAuthorization();

// 7. Hub ve Route Tanýmlamalarý
app.MapHub<VisitorHub>("/visitorHub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();