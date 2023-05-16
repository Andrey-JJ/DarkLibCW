using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarkLibCW.Models;
using DarkLibCW.Models.ViewModels;

namespace DarkLibCW.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDBContext _context;

        public BooksController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Books.Include(b => b.BookStatus).Include(b => b.CatalogCard);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookStatus)
                .Include(b => b.CatalogCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            IssuesOfBook bookIssues = new IssuesOfBook();
            bookIssues.Book = book;
            bookIssues.Issues = _context.Issues.Where(i => i.BookId == book.Id);

            return View(bookIssues);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            ViewData["CatalogCardId"] = new SelectList(_context.CatalogCards, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CatalogCardId,StatusId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", book.StatusId);
            ViewData["CatalogCardId"] = new SelectList(_context.CatalogCards, "Id", "Id", book.CatalogCardId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", book.StatusId);
            ViewData["CatalogCardId"] = new SelectList(_context.CatalogCards, "Id", "Id", book.CatalogCardId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatalogCardId,StatusId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", book.StatusId);
            ViewData["CatalogCardId"] = new SelectList(_context.CatalogCards, "Id", "Id", book.CatalogCardId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookStatus)
                .Include(b => b.CatalogCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'AppDBContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
