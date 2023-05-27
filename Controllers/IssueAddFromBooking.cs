using DarkLibCW.Areas.Identity.Data;
using DarkLibCW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var bookings = _context.Bookings;

            return View(bookings);
        }

        public async Task<IActionResult> CreateWithBooking(int BookId, int SubId, DateTime ReturnDate)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Librarian librarian = (Librarian)_context.Librarians.Where(l => l.UserName == User.Identity.Name);
            if (librarian == null) return NotFound();

            Issue issue = new Issue();

            issue.BookId = BookId;
            issue.SubscriberId = SubId;
            issue.LibrarianId = librarian.Id;
            issue.IssueDate = DateTime.Now.Date;
            issue.ReturnDate = ReturnDate.Date;

            _context.Issues.Add(issue);

            _context.SaveChangesAsync();

            return RedirectToAction("Index", "Issues");
        }
    }
}
