using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASM.Data;
using ASM.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ASMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASMContext") ?? throw new InvalidOperationException("Connection string 'ASMContext' not found.")));

//builder.Services.AddDbContext<ASMContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("ASMContext") ?? throw new InvalidOperationException("Connection string 'ASMContext' not found.")));
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(36000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//builder.Services.AddMvc().AddControllersAsServices();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cart}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Supplier",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
