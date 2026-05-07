namespace EventEase.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. Fix Multiple Cascade Paths ---
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany()
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany()
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- 2. Identity Seed Data ---

            // IDs for consistency
            string adminRoleId = "admin-role-id-constant";
            string specialistRoleId = "specialist-role-id-constant";
            string adminUserId = "admin-user-id-constant";
            string specialistUserId = "specialist-user-id-constant";

            // Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = specialistRoleId, Name = "Specialist", NormalizedName = "SPECIALIST" }
            );

            // Create Password Hasher
            var hasher = new PasswordHasher<IdentityUser>();

            // Seed Admin User
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@eventease.com",
                NormalizedUserName = "ADMIN@EVENTEASE.COM",
                Email = "admin@eventease.com",
                NormalizedEmail = "ADMIN@EVENTEASE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!"), // Change this password!
                SecurityStamp = string.Empty
            });

            // Seed Specialist User
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = specialistUserId,
                UserName = "specialist@eventease.com",
                NormalizedUserName = "SPECIALIST@EVENTEASE.COM",
                Email = "specialist@eventease.com",
                NormalizedEmail = "SPECIALIST@EVENTEASE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Specialist123!"), // Change this password!
                SecurityStamp = string.Empty
            });

            // Assign Users to Roles
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId },
                new IdentityUserRole<string> { RoleId = specialistRoleId, UserId = specialistUserId }
            );

            // --- 3. Seed Venues ---
            modelBuilder.Entity<Venue>().HasData(
                new Venue
                {
                    VenueId = 1,
                    VenueName = "Grand Ballroom",
                    Location = "Downtown",
                    Capacity = 500,
                    ImageUrl = "https://images.unsplash.com/photo-1519167758481-83f550bb49b3",
                    ContactEmail = "events@grand.com",
                    ContactPhone = "555-0101",
                    IsAvailable = true
                },
                new Venue
                {
                    VenueId = 2,
                    VenueName = "Skyline Terrace",
                    Location = "Rooftop",
                    Capacity = 150,
                    ImageUrl = "https://images.unsplash.com/photo-1533174072545-7a4b6ad7a6c3",
                    ContactEmail = "info@skyline.com",
                    ContactPhone = "555-0202",
                    IsAvailable = false
                }
            );
        }
    }
}