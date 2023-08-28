using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Web_App_Shop_V2.DAL;
using Web_App_Shop_V2;
using Web_App_Shop_V2.Service.Implementation;
using Web_App_Shop_V2.Service.Interfaces;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

//получение строки подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection");


//контекст ApplicationContext в качестве сервера
builder.Services.AddDbContext<AplicationDbContext>(options => options.UseNpgsql(connection));

//Аутентификация cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
});

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

