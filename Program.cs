using CarManagement.Data;
using CarManagement.Models;
using CarManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<DriversRepository>();
builder.Services.AddScoped<MaintenanceRepository>();
builder.Services.AddScoped<VehicleDriverHistoryRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MySqlConn");
builder.Services.AddDbContext<CarManagementContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add Identity and UI
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add Roles if you want
    .AddEntityFrameworkStores<CarManagementContext>()
    .AddDefaultUI(); // Make sure to include Identity UI

var app = builder.Build();

// Ensure the Authentication Middleware is used
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Make sure this is before authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Ensure Razor Pages are mapped for Identity UI

app.Run();
