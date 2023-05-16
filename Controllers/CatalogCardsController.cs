using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarkLibCW.Models;
using DarkLibCW.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace DarkLibCW.Controllers
{
    public class CatalogCardsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public CatalogCardsController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: CatalogCards
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.CatalogCards.Include(c => c.Category).Include(c => c.Edition);
            return View(await appDBContext.ToListAsync());
        }

        // GET: CatalogCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CatalogCards == null)
            {
                return NotFound();
            }

            var catalogCard = await _context.CatalogCards
                .Include(c => c.Category)
                .Include(c => c.Edition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catalogCard == null)
            {
                return NotFound();
            }

            if (!catalogCard.Image.IsNullOrEmpty())
            {
                byte[] imageData = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + catalogCard.Image);
                ViewBag.Image = imageData;
            }
            else { ViewBag.Image = null; }

            CardAndBooks cardAndBooks = new CardAndBooks();
            cardAndBooks.CatalogCard = catalogCard;
            cardAndBooks.Books = _context.Books.Where(b => b.CatalogCardId == catalogCard.Id);

            return View(cardAndBooks);
        }

        // GET: CatalogCards/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["EditionId"] = new SelectList(_context.Editions, "Id", "Id");
            return View();
        }

        // POST: CatalogCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,EditionId,EditionDate,Volume,Image,CategoryId")] CatalogCard catalogCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalogCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", catalogCard.CategoryId);
            ViewData["EditionId"] = new SelectList(_context.Editions, "Id", "Id", catalogCard.EditionId);
            return View(catalogCard);
        }

        // GET: CatalogCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CatalogCards == null)
            {
                return NotFound();
            }

            var catalogCard = await _context.CatalogCards.FindAsync(id);
            if (catalogCard == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", catalogCard.CategoryId);
            ViewData["EditionId"] = new SelectList(_context.Editions, "Id", "Id", catalogCard.EditionId);
            return View(catalogCard);
        }

        // POST: CatalogCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,EditionId,EditionDate,Volume,Image,CategoryId")] CatalogCard catalogCard)
        {
            if (id != catalogCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalogCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogCardExists(catalogCard.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", catalogCard.CategoryId);
            ViewData["EditionId"] = new SelectList(_context.Editions, "Id", "Id", catalogCard.EditionId);
            return View(catalogCard);
        }

        // GET: CatalogCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CatalogCards == null)
            {
                return NotFound();
            }

            var catalogCard = await _context.CatalogCards
                .Include(c => c.Category)
                .Include(c => c.Edition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catalogCard == null)
            {
                return NotFound();
            }

            return View(catalogCard);
        }

        // POST: CatalogCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CatalogCards == null)
            {
                return Problem("Entity set 'AppDBContext.CatalogCards'  is null.");
            }
            var catalogCard = await _context.CatalogCards.FindAsync(id);
            if (catalogCard != null)
            {
                _context.CatalogCards.Remove(catalogCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogCardExists(int id)
        {
          return (_context.CatalogCards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
