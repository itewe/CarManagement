using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarManagement.Data;
using CarManagement.Models;
using Microsoft.AspNetCore.Identity;
using CarManagement.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Repositories
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<DriversRepository>();
builder.Services.AddScoped<MaintenanceRepository>();
builder.Services.AddScoped<VehicleDriverHistoryRepository>();

// MySQL connection
var connectionString = builder.Configuration.GetConnectionString("MySqlConn");
builder.Services.AddDbContext<CarManagementContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add Roles for seeding
    .AddEntityFrameworkStores<CarManagementContext>()
    .AddDefaultUI(); // Identity UI

// Authorization policy for WebAdmin role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("WebAdminPolicy", policy => policy.RequireRole("WebAdmin"));
});

var app = builder.Build();

// Ensure the Authentication Middleware is used
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Ensure this is before authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Ensure Razor Pages are mapped for Identity UI

// Seed database (create roles and admin user)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await SeedData.Initialize(userManager, roleManager); // Call the SeedData.Initialize method
}

app.Run();
