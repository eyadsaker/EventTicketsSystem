using Eventsystem.Data;
using Eventsystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = _context.Reviews
                .Include(r => r.Event)
                .Include(r => r.User);

            return View(await reviews.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.Event)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return NotFound();

            return View(review);
        }

        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreatedAt = DateTime.Now;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(review);
            return View(review);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            PopulateDropDowns(review);
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(review);
            return View(review);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.Event)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return NotFound();

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDowns(Review? review = null)
        {
            ViewBag.EventId = new SelectList(_context.Events, "Id", "Title", review?.EventId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "FullName", review?.UserId);
        }
    }
}
