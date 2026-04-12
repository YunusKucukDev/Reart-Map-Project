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

builder.Services.AddControllersWithViews();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddSignalR();
builder.Services.AddSingleton<VisitorTracker>();
builder.Services.AddHttpClient<IVisitorLogService, VisitorLogService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

builder.Services.AddHttpClient<ICategoryService, CategoryService>(opt => {
    opt.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

builder.Services.AddHttpClient<IContactService, ContactService>();
builder.Services.AddHttpClient<IMapIdentityDescriptionService, MapIdentityDescriptionService>();
builder.Services.AddHttpClient<IUserInformationService, UserInformationService>();
builder.Services.AddHttpClient<IIdentityService, IdentityService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<VisitorHub>("/visitorHub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();