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

            // 1. Fix Multiple Cascade Paths
            // Booking -> Event (No Cascade)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany()
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            // Booking -> Venue (No Cascade)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany()
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.NoAction);

            // Event -> Venue (No Cascade)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.NoAction);

            // 2. Seed Roles (Your existing code)
            string adminRoleId = "admin-role-id-constant"; // Use a constant string for seeding to avoid duplicates
            string specialistRoleId = "specialist-role-id-constant";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = specialistRoleId, Name = "Specialist", NormalizedName = "SPECIALIST" }
            );

            // 3. Seed Venues (Your existing code)
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