using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarkLibCW.Models;
using DarkLibCW.Models.ViewModels;
using OfficeOpenXml;
using DarkLibCW.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DarkLibCW.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<DarkLibUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        public BooksController(AppDBContext context, UserManager<DarkLibUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Books.Include(b => b.Status).Include(b => b.CatalogCard);
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
                .Include(b => b.Status)
                .Include(b => b.CatalogCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            BookData bookData = new BookData();
            bookData.Book = book;
            bookData.Issues = _context.Issues.Where(i => i.BookId == id);
            bookData.Bookings = _context.Bookings.Where(I => I.BookId == id);

            return View(bookData);
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
                .Include(b => b.Status)
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

        public FileResult GetReport(int id)
        {
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var book = _context.Books
                .Include(b => b.CatalogCard)
                .FirstOrDefault(b => b.Id == id);
            BookData bookData = new BookData();
            bookData.Book = book;
            bookData.Issues = _context.Issues
                .Include(s => s.Subscriber)
                .Include(s => s.Librarian)
                .Where(i => i.BookId == id).ToList();
            bookData.Bookings = _context.Bookings
                .Include(s => s.Subscriber)
                .Where(b => b.BookId == id).ToList();

            // Путь к файлу с шаблоном
            string path = "/Reports/report_book_data.xlsx";
            //Путь к файлу с результатом
            string result = $"/Reports/report_{book.CatalogCard.Title}№{book.Id}.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);

            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Работник Р.Р.";
                excelPackage.Workbook.Properties.Title = "Список читателей книги";
                excelPackage.Workbook.Properties.Subject = "Пользователи";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Data"];
                int startLine = 3;
                foreach (var issue in bookData.Issues)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = issue.Id;
                    worksheet.Cells[startLine, 3].Value = issue.Subscriber.FullName;
                    worksheet.Cells[startLine, 4].Value = issue.Librarian.FullName;
                    worksheet.Cells[startLine, 5].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 6].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 5].Value = issue.IssueDate.Date;
                    worksheet.Cells[startLine, 6].Value = issue.ReturnDate.Date;
                }
                startLine = 3;
                foreach (var booking in bookData.Bookings)
                {
                    worksheet.Cells[startLine, 8].Value = startLine - 2;
                    worksheet.Cells[startLine, 9].Value = booking.Id;
                    worksheet.Cells[startLine, 10].Value = booking.Subscriber.FullName;
                }
                excelPackage.SaveAs(fr);
            }

            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = $"report_{book.CatalogCard.Title}№{book.Id}.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
