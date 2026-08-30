using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Eventsystem.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Categories.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _unitOfWork.Categories.GetByIdAsync(id.Value);

            if (category == null) return NotFound();

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _unitOfWork.Categories.GetByIdAsync(id.Value);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(category);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _unitOfWork.Categories.GetByIdAsync(id.Value);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category != null)
            {
                _unitOfWork.Categories.Delete(category);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}