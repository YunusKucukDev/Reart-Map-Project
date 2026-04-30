using MapProject.WebUI.Hubs;
using MapProject.WebUI.Services.EmailService;
using MapProject.WebUI.Services.CategoryService;
using MapProject.WebUI.Services.ContactService;
using MapProject.WebUI.Services.IdentityService;
using MapProject.WebUI.Services.MapIdentityDescriptionService;
using MapProject.WebUI.Services.UserInformationService;
using MapProject.WebUI.Services.CoureselService;
using MapProject.WebUI.Services.VideoService;
using MapProject.WebUI.Services.VisitorLogService;
using MapProject.WebUI.Services.VisitorService;
using Microsoft.AspNetCore.Authentication.Cookies;
using MapProject.WebUI.Services.MapViewerService;

var builder = WebApplication.CreateBuilder(args);

// 1. Temel Servisler
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<VisitorTracker>();

// 2. Email Ayarlar�
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// 3. Kimlik Do�rulama (Cookie) Ayarlar�
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

var apiUri = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);

builder.Services.AddHttpClient<ICategoryService, CategoryService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IContactService, ContactService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IMapIdentityDescriptionService, MapIdentityDescriptionService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IUserInformationService, UserInformationService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IIdentityService, IdentityService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IVisitorLogService, VisitorLogService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IMapViewerService, MapViewerService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<ICoureselService, CoureselService>(opt => opt.BaseAddress = apiUri);
builder.Services.AddHttpClient<IVideoService, VideoService>(opt => opt.BaseAddress = apiUri);

// 5. Uygulama Olu�turma
var app = builder.Build();

// 6. Middleware (Ara Yaz�l�m) Yap�land�rmas�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

// �nce Authentication (Kimlik Do�rulama), Sonra Authorization (Yetkilendirme)
app.UseAuthentication();
app.UseAuthorization();

// 7. Hub ve Route Tan�mlamalar�
app.MapHub<VisitorHub>("/visitorHub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();