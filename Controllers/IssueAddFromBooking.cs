using DarkLibCW.Areas.Identity.Data;
using DarkLibCW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarkLibCW.Controllers
{
    public class IssueAddFromBooking : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<DarkLibUser> _userManager;
        public IssueAddFromBooking(AppDBContext context, UserManager<DarkLibUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> ChooseBookingToIssue()
        {
            var bookings = _context.Bookings
                .Include(b => b.Book).ThenInclude(book => book.CatalogCard)
                .Include(b => b.Subscriber)
                .ToListAsync();
            return View(await bookings);
        }
        [HttpPost]
        public async Task<IActionResult> CreateWithBooking(int BookingId, int BookId, int SubId, DateTime ReturnDate)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Librarian librarian = (Librarian)_context.Librarians.Where(l => l.UserName == User.Identity.Name).FirstOrDefault();
            if (librarian == null) return NotFound();

            Booking booking = await _context.Bookings.FindAsync(BookingId);

            Book book = await _context.Books.FindAsync(BookId);
            book.StatusId = 2;

            Issue issue = new Issue();
            issue.BookId = BookId;
            issue.SubscriberId = SubId;
            issue.LibrarianId = librarian.Id;
            issue.IssueDate = DateTime.Now.Date;
            issue.ReturnDate = ReturnDate.Date;

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Issues");
        }
    }
}
