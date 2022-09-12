using Bulkybook.DataAcess.Repository;
using Bulkybook.DataAcess.Repository.IRepository;
using BulkyBookweb.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.Utility;
using Stripe;
using Bulkybook.DataAcess.DbIntializer;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//取得appsettings.json中Stripe的密鑰Class的屬性名稱必須與Section中的名稱相同
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//將身分認證由默認改為自訂認證，必須啟用默認令牌
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitialize>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();//註冊偽造的EmailSender
builder.Services.AddRazorPages();
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1059669684751458";
    options.AppSecret = "c1489eb6bfe8ecb078530f7103c07713";
});
//若未登入轉移到登入頁面
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath= $"/Identity/Account/Logout";
    options.AccessDeniedPath= $"/Identity/Account/AccessDenied";
});

//開啟Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential= true;
});
////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();//Session
app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>(); //assign the global Api key inside pipeline
SeedDatabase();
app.UseAuthentication();//UseAuthentication位置一定要優先於UseAuthorization

app.UseAuthorization();

app.MapRazorPages(); //使用Razorpage 要在pipeline中加入MapRazorPages
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}