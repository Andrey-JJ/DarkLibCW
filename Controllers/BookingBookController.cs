using DarkLibCW.Models.ViewModels;
using DarkLibCW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarkLibCW.Controllers
{
    public class BookingBookController : Controller
    {
        private readonly AppDBContext _context;
        //private readonly UserManager<LibraryUser> _userManager;
        public BookingBookController(AppDBContext context) //AppDBContext context, UserManager<LibraryUser> userManager
        {
            _context = context;
            //_userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> ChooseSub(int catalogid)
        {
            BookAndSubs bs = new BookAndSubs();

            bs.CardId = catalogid;
            bs.book = _context.Books.Where(book => book.CatalogCardId == catalogid && book.StatusId == 1).First();

            if (bs.book == null) return NotFound();

            bs.Subscribers = _context.Subscribers;

            if (bs.Subscribers == null) return NotFound();
            return View(bs);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmBooking(int bookId, int subId, int cardId)
        {
            CatalogCard card = _context.CatalogCards.Find(cardId);
            Subscriber sub = _context.Subscribers.Find(subId);

            ViewBag.BookId = bookId;
            ViewBag.Sub = sub.FullName;
            ViewBag.SubId = subId;
            ViewBag.Card = card.Title;
            ViewBag.CardId = cardId;
            return View("ConfirmBooking");
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(int BookId, int SubId, int CardId)
        {

            Booking booking = new Booking();
            booking.BookId = BookId;
            booking.SubscriberId = SubId;
            _context.Add(booking);
            await _context.SaveChangesAsync();

            var book = _context.Books.Find(BookId);
            book.StatusId = 2;
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Bookings");
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //if (await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "librarian"))
            //    return RedirectToAction("Index", "Bookings");
            //else
            //    return RedirectToAction("Details", "CatalogCard", new { id = CardId });
        }
    }
}
