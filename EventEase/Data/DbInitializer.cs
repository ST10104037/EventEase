using EventEase.Models;
using Microsoft.AspNetCore.Identity;

namespace EventEase.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // 1. Seed Roles
            string[] roleNames = { "Admin", "BookingSpecialist", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Admin User
            if (await userManager.FindByEmailAsync("admin@eventease.com") == null)
            {
                var adminUser = new IdentityUser { UserName = "admin@eventease.com", Email = "admin@eventease.com", EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, "Password123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 3. Seed Venues & Events (Only if empty)
            if (!context.Venues.Any())
            {
                context.Venues.AddRange(
                    new Venue { VenueName = "Grand Hall", Location = "Johannesburg", Capacity = 500, ImageUrl = "..." },
                    new Venue { VenueName = "Sunset Gardens", Location = "Cape Town", Capacity = 200, ImageUrl = "..." }
                );
                await context.SaveChangesAsync();
            }
            // Add similar logic for Events and Bookings...
        }
    }
}