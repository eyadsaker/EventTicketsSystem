using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventsystem.Controllers
{
    public class TicketTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _unitOfWork.TicketTypes.GetTicketTypesWithEventAsync();
            return View(types);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var type = await _unitOfWork.TicketTypes.GetTicketTypeDetailsAsync(id.Value);

            if (type == null) return NotFound();

            return View(type);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.TicketTypes.AddAsync(ticketType);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(ticketType);
            return View(ticketType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var type = await _unitOfWork.TicketTypes.GetByIdAsync(id.Value);
            if (type == null) return NotFound();

            await PopulateDropDowns(type);
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketType ticketType)
        {
            if (id != ticketType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.TicketTypes.Update(ticketType);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(ticketType);
            return View(ticketType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var type = await _unitOfWork.TicketTypes.GetTicketTypeDetailsAsync(id.Value);

            if (type == null) return NotFound();

            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _unitOfWork.TicketTypes.GetByIdAsync(id);
            if (type != null)
            {
                _unitOfWork.TicketTypes.Delete(type);
                await _unitOfWork.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDowns(TicketType? ticketType = null)
        {
            var events = await _unitOfWork.Events.GetAllAsync();
            ViewBag.EventId = new SelectList(events, "Id", "Title", ticketType?.EventId);
        }
    }
}
