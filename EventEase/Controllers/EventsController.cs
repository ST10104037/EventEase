using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    [Authorize(Roles = "Admin, Specialist")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobService;

        public EventsController(ApplicationDbContext context, BlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // --- PUBLIC ACCESS ALLOWED HERE ---
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var events = _context.Events.Include(e => e.Venue);
            return View(await events.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // --- RESTRICTED TO ADMIN ONLY BEYOND THIS POINT ---

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,Description,VenueId")] Event @event, IFormFile? imageFile)
        {
            // Remove Venue from validation because it's a navigation property, not a form input
            ModelState.Remove("Venue");

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    @event.ImageUrl = await _blobService.UploadAsync(imageFile);
                }

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId,ImageUrl,Contact")] Event @event, IFormFile? imageFile)
        {
            if (id != @event.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        @event.ImageUrl = await _blobService.UploadAsync(imageFile);
                    }

                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1. Check if any bookings are linked to this event
            bool hasLinkedBookings = await _context.Bookings.AnyAsync(b => b.EventId == id);

            if (hasLinkedBookings)
            {
                // 2. Instead of crashing, send a friendly alert back to the Delete view
                TempData["ErrorMessage"] = "Cannot delete this event because it has active bookings. Please delete the bookings first.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}