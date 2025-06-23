using EventEaseBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EventEaseBookingSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EventController> _logger;

        public EventController(AppDbContext context, ILogger<EventController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Event with filtering
        public async Task<IActionResult> Index(string searchType, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchType))
            {
                events = events.Where(e => e.EventType.EventTypeName.Contains(searchType));
            }

            if (venueId.HasValue)
            {
                events = events.Where(e => e.VenueId == venueId);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                events = events.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);
            }

            // Setup view data for filters
            ViewData["EventTypes"] = new SelectList(_context.EventType, "EventTypeId", "EventTypeName");
            ViewData["Venues"] = new SelectList(_context.Venue, "VenueId", "VenueName");

            return View(await events.ToListAsync());
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            SetupEventDropdowns();
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventName,EventDate,Description,VenueId,EventTypeId")] Event eventItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(eventItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error creating event");
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                }
            }

            SetupEventDropdowns();
            return View(eventItem);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Event.FindAsync(id);
            if (eventItem == null) return NotFound();

            SetupEventDropdowns(eventItem.VenueId, eventItem.EventTypeId);
            return View(eventItem);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,Description,VenueId,EventTypeId")] Event eventItem)
        {
            if (id != eventItem.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!EventExists(eventItem.EventId))
                    {
                        return NotFound();
                    }
                    _logger.LogError(ex, "Error editing event");
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                }
            }

            SetupEventDropdowns(eventItem.VenueId, eventItem.EventTypeId);
            return View(eventItem);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null) return NotFound();

            return View(eventItem);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Event.FindAsync(id);
            if (eventItem == null) return NotFound();

            try
            {
                _context.Event.Remove(eventItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting event");
                TempData["ErrorMessage"] = "Error deleting event. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }

        private void SetupEventDropdowns(int? selectedVenueId = null, int? selectedEventTypeId = null)
        {
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", selectedVenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventType, "EventTypeId", "EventTypeName", selectedEventTypeId);
        }
    }
}