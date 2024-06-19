using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IMAGIO.Data;
using IMAGIO.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IMAGIO.Services;
using IMAGIO.Interfaces;
using IMAGIO;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using Microsoft.CodeAnalysis;
using Weikio.PluginFramework.AspNetCore;
using IPlugin;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
var connectionString = "server=localhost;UserId=root;Password=root;database=imagio";

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 11))));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });

builder.Services.AddPluginFramework<Iplugin>(@".\plugins");

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
