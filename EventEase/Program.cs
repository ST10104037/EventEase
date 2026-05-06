using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity Service Registration (Requires Microsoft.AspNetCore.Identity.UI package)
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false; // Optional: Makes testing easier
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

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