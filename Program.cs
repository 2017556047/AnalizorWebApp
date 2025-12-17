using AnalizorWebApp.Data;
using AnalizorWebApp.Hubs;
using AnalizorWebApp.Models;
using AnalizorWebApp.Services;
using AnalizorWebApp.Workers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// MVC + GLOBAL AUTHORIZE
// Login harici her yer kimlik doðrulama ister
// ------------------------------------------------------
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages();

// ------------------------------------------------------
// DB
// ------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------------------------------------
// IDENTITY
// ------------------------------------------------------
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ------------------------------------------------------
// COOKIE AYARLARI
// ------------------------------------------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;

    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// ------------------------------------------------------
// SIGNALR & SERVICES
// ------------------------------------------------------
builder.Services.AddSignalR();
builder.Services.AddHostedService<DevicePoller>();
builder.Services.AddHostedService<EnergySnapshotWorker>();
builder.Services.AddScoped<ModbusService>();

var app = builder.Build();

// ------------------------------------------------------
// PIPELINE
// ------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ------------------------------------------------------
// ROUTES
// ------------------------------------------------------
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ------------------------------------------------------
// ROLE & USER SEED
// ------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await DbInitializer.SeedAsync(roleManager, userManager);
}

// ------------------------------------------------------
// SIGNALR HUB
// ------------------------------------------------------
app.MapHub<LiveDataHub>("/liveDataHub");

app.Run();
