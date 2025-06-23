using EventEaseBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EventEaseBookingSystem.Services;

namespace EventEaseBookingSystem.Controllers
{
    public class VenueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AzureBlobStorageService _blobService;

        public VenueController(AppDbContext context, AzureBlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venue search
        public async Task<IActionResult> Index(string searchString)
        {
            var venues = from v in _context.Venue select v;

            if (!string.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(v =>
                    v.VenueName.Contains(searchString) ||
                    v.Location.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await venues.AsNoTracking().ToListAsync());
        }

        // GET: Venue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        // GET: Venue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {


            if (ModelState.IsValid)//Checks if all the fields have filled out correctly before adding new data 
            {
                if (venue.ImageFile != null)
                {
                    var blobUrl = await _blobService.UploadFileAsync(venue.ImageFile, "venues");
                    venue.ImageUrl = blobUrl;
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));

            }

            return View(venue);
        }

        // GET: Venue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVenue = await _context.Venue
                        .AsNoTracking()
                        .FirstOrDefaultAsync(v => v.VenueId == id);

                    if (venue.ImageFile != null)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingVenue?.ImageUrl))
                        {
                            await _blobService.DeleteFileAsync(existingVenue.ImageUrl);
                        }

                        venue.ImageUrl = await _blobService.UploadFileAsync(venue.ImageFile, "venues");
                    }
                    else
                    {
                        venue.ImageUrl = existingVenue?.ImageUrl;
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId)) return NotFound();
                    throw;
                }
            }
            return View(venue);
        }

        // GET: Venue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        // POST: Venue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) return NotFound();

            // Check for active bookings
            bool hasActiveBookings = await _context.Booking
                .AnyAsync(b => b.VenueId == id && b.BookingDate >= DateTime.Today);

            if (hasActiveBookings)
            {
                TempData["Error"] = "Cannot delete venue as it has active/future bookings.";
                return RedirectToAction(nameof(Index));
            }

            // Delete associated image from blob storage
            if (!string.IsNullOrEmpty(venue.ImageUrl))
            {
                await _blobService.DeleteFileAsync(venue.ImageUrl);
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }
    }
}