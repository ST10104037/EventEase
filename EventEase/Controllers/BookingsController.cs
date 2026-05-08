using EventEase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    [Authorize(Roles = "Admin, Specialist")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var bookingsQuery = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Use .Contains for the name, and only parse the ID if the string is numeric
                if (int.TryParse(searchString, out int idSearch))
                {
                    bookingsQuery = bookingsQuery.Where(b => b.BookingId == idSearch || b.Event.EventName.Contains(searchString));
                }
                else
                {
                    bookingsQuery = bookingsQuery.Where(b => b.Event.EventName.Contains(searchString));
                }
            }

            return View(await bookingsQuery.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            // Fix: Explicitly remove navigation properties from validation
            ModelState.Remove("Event");
            ModelState.Remove("Venue");

            // 1. Fetch the associated event to check its scheduled time
            var selectedEvent = await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            DateTime now = DateTime.Now;

            // 2. Double Booking Check
            bool isDoubleBooked = _context.Bookings.Any(b =>
                b.VenueId == booking.VenueId &&
                b.BookingDate.Date == booking.BookingDate.Date);

            if (isDoubleBooked)
                ModelState.AddModelError("BookingDate", "This venue is already booked on the selected date.");

            // 3. Time Validation Logic
            if (selectedEvent != null)
            {
                // Rule: No bookings for the past or today (must be at least 1 day in advance)
                if (selectedEvent.EventDate.Date <= now.Date)
                {
                    ModelState.AddModelError("", "Bookings must be made at least 24 hours in advance. Same-day or past bookings are not permitted.");
                }
                
                // Rule: Block if within 1 hour of the event start
                double hoursRemaining = (selectedEvent.EventDate - now).TotalHours;
                if (hoursRemaining > 0 && hoursRemaining < 1)
                {
                    ModelState.AddModelError("", "Emergency Lock: Cannot create a booking within 1 hour of the event start time.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking confirmed!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        // Only Admins can edit historical booking data
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            // Fix: Explicitly remove navigation properties from validation
            ModelState.Remove("Event");
            ModelState.Remove("Venue");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);

        }

        // Specialists and Admins can delete/cancel bookings
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            return booking == null ? NotFound() : View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the specific booking we want to delete
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            // Logic: Just remove the booking. 
            // (You don't need to check hasActiveBookings here because this IS the booking you are deleting!)
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking successfully cancelled.";
            return RedirectToAction(nameof(Index));
        }
        private bool BookingExists(int id) => _context.Bookings.Any(e => e.BookingId == id);
    }
}