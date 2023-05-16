using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarkLibCW.Models;

namespace DarkLibCW.Controllers
{
    public class IssuesController : Controller
    {
        private readonly AppDBContext _context;

        public IssuesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Issues.Include(i => i.Book).Include(i => i.Librarian).Include(i => i.Subscriber);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Issues == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Librarian)
                .Include(i => i.Subscriber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            ViewData["LibrarianId"] = new SelectList(_context.Librarians, "Id", "Id");
            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Id");
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,SubscriberId,LibrarianId,IssueDate,ReturnDate")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", issue.BookId);
            ViewData["LibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", issue.LibrarianId);
            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Id", issue.SubscriberId);
            return View(issue);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Issues == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", issue.BookId);
            ViewData["LibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", issue.LibrarianId);
            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Id", issue.SubscriberId);
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,SubscriberId,LibrarianId,IssueDate,ReturnDate")] Issue issue)
        {
            if (id != issue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", issue.BookId);
            ViewData["LibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", issue.LibrarianId);
            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Id", issue.SubscriberId);
            return View(issue);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Issues == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .Include(i => i.Book)
                .Include(i => i.Librarian)
                .Include(i => i.Subscriber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Issues == null)
            {
                return Problem("Entity set 'AppDBContext.Issues'  is null.");
            }
            var issue = await _context.Issues.FindAsync(id);
            if (issue != null)
            {
                _context.Issues.Remove(issue);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
          return (_context.Issues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
