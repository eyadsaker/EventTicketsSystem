using Eventsystem.Data;
using Eventsystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Controllers
{
    public class TicketTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var types = _context.TicketTypes.Include(t => t.Event);
            return View(await types.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var type = await _context.TicketTypes
                .Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (type == null) return NotFound();

            return View(type);
        }

        public IActionResult Create()
        {
            ViewBag.EventId = new SelectList(_context.Events, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventId = new SelectList(_context.Events, "Id", "Title", ticketType.EventId);
            return View(ticketType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var type = await _context.TicketTypes.FindAsync(id);
            if (type == null) return NotFound();

            ViewBag.EventId = new SelectList(_context.Events, "Id", "Title", type.EventId);
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketType ticketType)
        {
            if (id != ticketType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventId = new SelectList(_context.Events, "Id", "Title", ticketType.EventId);
            return View(ticketType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var type = await _context.TicketTypes
                .Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (type == null) return NotFound();

            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _context.TicketTypes.FindAsync(id);
            if (type != null)
            {
                _context.TicketTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
