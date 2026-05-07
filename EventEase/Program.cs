using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Do NOT use AddDefaultUI() if you want to completely block scaffolded registration.
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Optional but recommended: Block the /Register route entirely using an endpoint filter or redirect in your startup configuration.

// 3. Custom Services
builder.Services.AddScoped<BlobStorageService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Ensure Identity Middleware is used
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Required for Identity's default UI pages

app.Run();