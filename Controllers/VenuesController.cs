using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eventsystem.Controllers
{
    public class VenuesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VenuesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Venues.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _unitOfWork.Venues.GetByIdAsync(id.Value);

            if (venue == null) return NotFound();

            return View(venue);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Venues.AddAsync(venue);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _unitOfWork.Venues.GetByIdAsync(id.Value);

            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Venues.Update(venue);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _unitOfWork.Venues.GetByIdAsync(id.Value);

            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(id);

            if (venue != null)
            {
                _unitOfWork.Venues.Delete(venue);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
