using Eventsystem.Models;
using Eventsystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventsystem.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsWithDetailsAsync();
            return View(reviews);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _unitOfWork.Reviews.GetReviewDetailsAsync(id.Value);

            if (review == null) return NotFound();

            return View(review);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreatedAt = DateTime.Now;

                await _unitOfWork.Reviews.AddAsync(review);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(review);
            return View(review);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var review = await _unitOfWork.Reviews.GetByIdAsync(id.Value);
            if (review == null) return NotFound();

            await PopulateDropDowns(review);
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Reviews.Update(review);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(review);
            return View(review);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var review = await _unitOfWork.Reviews.GetReviewDetailsAsync(id.Value);

            if (review == null) return NotFound();

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review != null)
            {
                _unitOfWork.Reviews.Delete(review);
                await _unitOfWork.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDowns(Review? review = null)
        {
            var events = await _unitOfWork.Events.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();

            ViewBag.EventId = new SelectList(events, "Id", "Title", review?.EventId);
            ViewBag.UserId = new SelectList(users, "Id", "FullName", review?.UserId);
        }
    }
}
