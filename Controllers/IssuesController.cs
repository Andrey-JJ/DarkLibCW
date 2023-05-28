using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarkLibCW.Models;
using OfficeOpenXml;
using DarkLibCW.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using DarkLibCW.Models.ViewModels;

namespace DarkLibCW.Controllers
{
    public class IssuesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<DarkLibUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        public IssuesController(AppDBContext context, UserManager<DarkLibUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Issues
                .Include(i => i.Book).ThenInclude(b => b.CatalogCard)
                .Include(i => i.Librarian)
                .Include(i => i.Subscriber);
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
                .Include(i => i.Book).ThenInclude(b => b.CatalogCard)
                .Include(i => i.Book).ThenInclude(b => b.Status)
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

                var book = _context.Books.Find(issue.BookId);
                book.StatusId = 3;
                _context.Update(book);
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

        public FileResult GetReport()
        {
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var issues = _context.Issues
                .Include(i => i.Book).ThenInclude(b => b.CatalogCard)
                .Include(i => i.Subscriber)
                .Include(i => i.Librarian)
                .ToList();

            string path = "/Reports/report_issues.xlsx";
            //Путь к файлу с результатом
            string result = $"/Reports/report_issuesAll.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);

            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Работник Р.Р.";
                excelPackage.Workbook.Properties.Title = "Список выдачи за 30 дней";
                excelPackage.Workbook.Properties.Subject = "Выдачи";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Issues"];
                int startLine = 3;
                foreach (var item in issues)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = item.Id;
                    worksheet.Cells[startLine, 3].Value = item.Book.CatalogCard.Title;
                    worksheet.Cells[startLine, 4].Value = item.Subscriber.FullName;
                    worksheet.Cells[startLine, 5].Value = item.Librarian.FullName;
                    worksheet.Cells[startLine, 6].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 7].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 6].Value = item.IssueDate.Date;
                    worksheet.Cells[startLine, 7].Value = item.ReturnDate.Date;
                }

                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = $"report_issuesAll.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
