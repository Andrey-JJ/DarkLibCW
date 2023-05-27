using DarkLibCW.Models.ViewModels;
using DarkLibCW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DarkLibCW.Areas.Identity.Data;

namespace DarkLibCW.Controllers
{
    public class BookingBookController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<DarkLibUser> _userManager;
        public BookingBookController(AppDBContext context, UserManager<DarkLibUser> userManager) //
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> ChooseSub(int catalogid)
        {
            //Для библиотекаря
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "librarian"))
            {
                BookAndSubs bs = new BookAndSubs();

                bs.CardId = catalogid;
                bs.book = _context.Books.Where(book => book.CatalogCardId == catalogid && book.StatusId == 1).First();

                if (bs.book == null) return NotFound();

                bs.Subscribers = _context.Subscribers;

                if (bs.Subscribers == null) return NotFound();

                return View(bs);
            }
            //Для читателя
            else
            {
                BookAndSub bs = new BookAndSub();

                bs.CardId = catalogid;
                bs.Book = _context.Books.Where(book => book.CatalogCardId == catalogid && book.StatusId == 1).First();

                if (bs.Book == null) return NotFound();

                bs.Subscriber = (Subscriber)_context.Subscribers.Where(s => s.UserName == User.Identity.Name).FirstOrDefault();
                if (bs.Subscriber == null) return NotFound();
                return RedirectToAction("ConfirmBooking", "BookingBook", new { BookId = bs.Book.Id, SubId = bs.Subscriber.Id, CardId = catalogid });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmBooking(int BookId, int SubId, int CardId)
        {
            CatalogCard card = _context.CatalogCards.Find(CardId);
            Subscriber sub = _context.Subscribers.Find(SubId);

            ViewBag.BookId = BookId;
            ViewBag.Sub = sub;
            ViewBag.Card = card;
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
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "admin") || await _userManager.IsInRoleAsync(user, "librarian"))
                return RedirectToAction("Index", "Bookings");
            else
                return RedirectToAction("Details", "CatalogCard", new { id = CardId });

        }
    }
}
