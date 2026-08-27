using Eventsystem.Data;
using Eventsystem.Models;
using Eventsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userBookings = await _context.Bookings
                .Include(b => b.Items)
                    .ThenInclude(i => i.TicketType)
                        .ThenInclude(t => t.Event)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(userBookings);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.TicketType)
                        .ThenInclude(t => t.Event) 
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
        public async Task<IActionResult> Create(BookingCheckoutVM model)
        {
            if (model.Quantity <= 0 || model.TicketTypeId <= 0)
            {
                TempData["Error"] = "Please select the correct ticket and quantity.";
                return RedirectToAction("Index", "Events");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ticketType = await _context.TicketTypes.FindAsync(model.TicketTypeId);
                if (ticketType == null)
                {
                    return NotFound();
                }

                if (ticketType.Available < model.Quantity)
                {
                    TempData["Error"] = "Sorry, the requested quantity is currently unavailable.";
                    return RedirectToAction("Details", "Events", new { id = ticketType.EventId });
                }

                string reference = $"EVT-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

                var booking = new Booking
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                    BookingReference = reference,
                    BookingDate = DateTime.Now,
                    TotalAmount = ticketType.Price * model.Quantity,
                    Status = BookingStatus.Confirmed
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                var bookingItem = new BookingItem
                {
                    BookingId = booking.Id,
                    TicketTypeId = ticketType.Id,
                    Quantity = model.Quantity,
                    UnitPrice = ticketType.Price
                };

                _context.BookingItems.Add(bookingItem);

                ticketType.SoldQuantity += model.Quantity;
                _context.Update(ticketType);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Tickets have been successfully booked";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "There was a booking conflict another person booked the same tickets at the same time Please try again.";
                return RedirectToAction("Index", "Events");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
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