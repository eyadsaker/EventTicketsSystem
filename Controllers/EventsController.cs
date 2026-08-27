using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Eventsystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventsystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _unitOfWork.Events
                .GetEventsWithDetailsAsync();

            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ev = await _unitOfWork.Events
                .GetEventDetailsAsync(id.Value);

            if (ev == null)
                return NotFound();

            var viewModel = new EventDetailsVM
            {
                Event = ev,

                TicketTypes = ev.TicketTypes.Select(t => new TicketTypeVM
                {
                    Id = t.Id,
                    Name = t.Name,
                    Price = t.Price,
                    AvailableQuantity = t.Available,
                    SelectedQuantity = 1
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event ev)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Events.AddAsync(ev);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(ev);

            return View(ev);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ev = await _unitOfWork.Events.GetByIdAsync(id.Value);

            if (ev == null)
                return NotFound();

            await PopulateDropDowns(ev);

            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event ev)
        {
            if (id != ev.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Events.Update(ev);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(ev);

            return View(ev);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ev = await _unitOfWork.Events.GetEventDetailsAsync(id.Value);

            if (ev == null)
                return NotFound();

            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _unitOfWork.Events.GetByIdAsync(id);

            if (ev != null)
            {
                _unitOfWork.Events.Delete(ev);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDowns(Event? ev = null)
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var venues = await _unitOfWork.Venues.GetAllAsync();

            ViewBag.CategoryId = new SelectList(
                categories,
                "Id",
                "Name",
                ev?.CategoryId
            );

            ViewBag.VenueId = new SelectList(
                venues,
                "Id",
                "Name",
                ev?.VenueId
            );
        }
    }
}
