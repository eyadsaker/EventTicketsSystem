using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Eventsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eventsystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userBookings = await _unitOfWork.Bookings.GetUserBookingsAsync(userId!);

            return View(userBookings);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _unitOfWork.Bookings.GetBookingDetailsAsync(id.Value);

            if (booking == null) return NotFound();

            return View(booking);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            ViewBag.UserId = new SelectList(users, "Id", "FullName");
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

            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var ticketType = await _unitOfWork.TicketTypes.GetByIdAsync(model.TicketTypeId);
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

                await _unitOfWork.Bookings.AddAsync(booking);
                await _unitOfWork.SaveAsync();

                var bookingItem = new BookingItem
                {
                    BookingId = booking.Id,
                    TicketTypeId = ticketType.Id,
                    Quantity = model.Quantity,
                    UnitPrice = ticketType.Price
                };

                await _unitOfWork.BookingItems.AddAsync(bookingItem);

                ticketType.SoldQuantity += model.Quantity;
                _unitOfWork.TicketTypes.Update(ticketType);

                await _unitOfWork.SaveAsync();
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

            var booking = await _unitOfWork.Bookings.GetByIdAsync(id.Value);
            if (booking == null) return NotFound();

            var users = await _unitOfWork.Users.GetAllAsync();
            ViewBag.UserId = new SelectList(users, "Id", "FullName", booking.UserId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Bookings.Update(booking);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            var users = await _unitOfWork.Users.GetAllAsync();
            ViewBag.UserId = new SelectList(users, "Id", "FullName", booking.UserId);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _unitOfWork.Bookings.GetBookingDetailsAsync(id.Value);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking != null)
            {
                _unitOfWork.Bookings.Delete(booking);
                await _unitOfWork.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}