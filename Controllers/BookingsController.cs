using Eventsystem.Data;
using Eventsystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Items);

            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.TicketType)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        public IActionResult Create()
        {
            ViewBag.UserId = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.BookingReference = $"EVT-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
                booking.BookingDate = DateTime.Now;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserId = new SelectList(_context.Users, "Id", "FullName", booking.UserId);
            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.UserId = new SelectList(_context.Users, "Id", "FullName", booking.UserId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserId = new SelectList(_context.Users, "Id", "FullName", booking.UserId);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
