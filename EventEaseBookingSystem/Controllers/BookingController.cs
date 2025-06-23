using EventEaseBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventEaseBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(AppDbContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Booking
        public async Task<IActionResult> Index(string searchString)
        {
            var bookings = _context.Booking
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.BookingId.ToString().Contains(searchString) ||
                    b.Venue.VenueName.Contains(searchString) ||
                    b.Event.EventName.Contains(searchString) ||
                    b.Event.EventDate.ToString().Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await bookings.OrderByDescending(b => b.BookingDate).ToListAsync());
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName");
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            try
            {
                var selectedEvent = await _context.Event.FindAsync(booking.EventId);
                if (selectedEvent == null)
                {
                    ModelState.AddModelError("EventId", "Selected event not found.");
                    RefreshDropdowns();
                    return View(booking);
                }

                // Check for booking conflicts
                var conflict = await _context.Booking
                    .Include(b => b.Event)
                    .AnyAsync(b => b.VenueId == booking.VenueId &&
                                 b.Event.EventDate.Date == selectedEvent.EventDate.Date);

                if (conflict)
                {
                    ModelState.AddModelError("", "This venue is already booked for that date.");
                    RefreshDropdowns();
                    return View(booking);
                }

                if (ModelState.IsValid)
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating booking");
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
            }

            RefreshDropdowns();
            return View(booking);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking        
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    _logger.LogError(ex, "Error editing booking");
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                }
            }

            RefreshDropdowns(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var booking = await _context.Booking.FindAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }

                _context.Booking.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting booking");
                TempData["ErrorMessage"] = "Error deleting booking. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }

        private void RefreshDropdowns(int? eventId = null, int? venueId = null)
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName", venueId);
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName", eventId);
        }
    }
}