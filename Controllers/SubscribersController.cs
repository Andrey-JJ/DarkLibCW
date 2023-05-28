using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DarkLibCW.Models;
using DarkLibCW.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DarkLibCW.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;

namespace DarkLibCW.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<DarkLibUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        public SubscribersController(AppDBContext context, UserManager<DarkLibUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        // GET: Subscribers
        public async Task<IActionResult> Index()
        {
              return _context.Subscribers != null ? 
                          View(await _context.Subscribers.ToListAsync()) :
                          Problem("Entity set 'AppDBContext.Subscribers'  is null.");
        }

        // GET: Subscribers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriber == null)
            {
                return NotFound();
            }

            SubscriberBooks data = new SubscriberBooks();
            data.Subscriber = subscriber;
            data.Issues = _context.Issues
                .Include(s => s.Book).ThenInclude(b => b.CatalogCard)
                .Where(s => s.SubscriberId == subscriber.Id).ToList();
            data.Bookings = _context.Bookings
                .Include(s => s.Book).ThenInclude(b => b.CatalogCard)
                .Where(s => s.SubscriberId == subscriber.Id).ToList();

            return View(data);
        }

        [Authorize(Roles = "librarian,admin")]
        // GET: Subscribers/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Subscribers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,Name,MidName,UserName")] Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscriber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subscriber);
        }

        [Authorize(Roles = "librarian,admin")]
        // GET: Subscribers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber == null)
            {
                return NotFound();
            }
            return View(subscriber);
        }

        [Authorize(Roles = "librarian,admin")]
        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,Name,MidName,UserName")] Subscriber subscriber)
        {
            if (id != subscriber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscriber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriberExists(subscriber.Id))
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
            return View(subscriber);
        }

        [Authorize(Roles = "librarian,admin")]
        // GET: Subscribers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        [Authorize(Roles = "librarian,admin")]
        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscribers == null)
            {
                return Problem("Entity set 'AppDBContext.Subscribers'  is null.");
            }
            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber != null)
            {
                _context.Subscribers.Remove(subscriber);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriberExists(int id)
        {
          return (_context.Subscribers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public FileResult GetReport(int? id)
        {
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var subscriber = _context.Subscribers.FirstOrDefault(s => s.Id == id);
            SubscriberBooks data = new SubscriberBooks();
            data.Subscriber = subscriber;
            data.Issues = _context.Issues
                .Include(s => s.Book).ThenInclude(b => b.CatalogCard)
                .Where(s => s.SubscriberId == subscriber.Id).ToList();
            data.Bookings = _context.Bookings
                .Include(s => s.Book).ThenInclude(b => b.CatalogCard)
                .Where(s => s.SubscriberId == subscriber.Id).ToList();

            // Путь к файлу с шаблоном
            string path = "/Reports/report_sub_books.xlsx";
            //Путь к файлу с результатом
            string result = $"/Reports/report_{subscriber.UserName}_books.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);

            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = subscriber.ShortName;
                excelPackage.Workbook.Properties.Title = "Список книг пользователя";
                excelPackage.Workbook.Properties.Subject = "Книги";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Books"];
                int startLine = 3;
                foreach(var issue in data.Issues)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = issue.Id;
                    worksheet.Cells[startLine, 3].Value = issue.Book.CatalogCard.Title;
                    worksheet.Cells[startLine, 4].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 5].Style.Numberformat.Format = "dd.MM.yyyy";
                    worksheet.Cells[startLine, 4].Value = issue.IssueDate.Date;
                    worksheet.Cells[startLine, 5].Value = issue.ReturnDate.Date;
                }
                startLine = 3;
                foreach (var booking in data.Bookings)
                {
                    worksheet.Cells[startLine, 9].Value = startLine - 2;
                    worksheet.Cells[startLine, 10].Value = booking.Id;
                    worksheet.Cells[startLine, 11].Value = booking.Book.CatalogCard.Title;
                }
                excelPackage.SaveAs(fr);
            }
            // Тип файла - content-type
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            // Имя файла - необязательно
            string file_name = $"report_{subscriber.UserName}_books.xlsx";
            return File(result, file_type, file_name);
        }
    }
}
