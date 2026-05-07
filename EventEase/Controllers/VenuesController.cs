using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    [Authorize(Roles = "Admin, Specialist")]
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobService;

        public VenuesController(ApplicationDbContext context, BlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // --- PUBLIC ACCESS ALLOWED HERE ---
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        // --- RESTRICTED TO ADMIN ONLY BEYOND THIS POINT ---

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // --- POST: Venues/Create ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // Added ContactEmail and ContactPhone to the Bind list
        public async Task<IActionResult> Create([Bind("VenueId,VenueName,Location,Capacity,ContactEmail,ContactPhone")] Venue venue, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    venue.ImageUrl = await _blobService.UploadAsync(imageFile);
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // DEBUG: If you hit this point, validation failed. 
            // Check your Venue model for [Required] fields you missed.
            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // --- POST: Venues/Edit ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,ImageUrl,ContactEmail,ContactPhone")] Venue venue, IFormFile? imageFile)
        {
            if (id != venue.VenueId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Upload new image if provided
                        venue.ImageUrl = await _blobService.UploadAsync(imageFile);
                    }
                    // If no new image, it keeps the hidden ImageUrl from the form

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            // Check if there are any bookings for this venue
            bool hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);

            if (venue != null)
            {
                _context.Venues.Remove(venue);
            }
            if (hasBookings)
            {
                // Add a model error that we can display as an alert
                TempData["ErrorMessage"] = "Cannot delete venue: It has active bookings associated with it.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}