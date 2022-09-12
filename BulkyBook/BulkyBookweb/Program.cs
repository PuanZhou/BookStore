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

//���oappsettings.json��Stripe���K�_Class���ݩʦW�٥����PSection�����W�٬ۦP
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//�N�����{�ҥ��q�{�אּ�ۭq�{�ҡA�����ҥ��q�{�O�P
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitialize>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();//���U���y��EmailSender
builder.Services.AddRazorPages();
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1059669684751458";
    options.AppSecret = "c1489eb6bfe8ecb078530f7103c07713";
});
//�Y���n�J�ಾ��n�J����
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath= $"/Identity/Account/Logout";
    options.AccessDeniedPath= $"/Identity/Account/AccessDenied";
});

//�}��Session
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
app.UseAuthentication();//UseAuthentication��m�@�w�n�u����UseAuthorization

app.UseAuthorization();

app.MapRazorPages(); //�ϥ�Razorpage �n�bpipeline���[�JMapRazorPages
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