using DarkLibCW.Areas.Identity.Data;
using DarkLibCW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarkLibCW.Controllers
{
    public class AddManyBooks : Controller
    {
        private readonly AppDBContext _context;
        public AddManyBooks(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetCountsOfBooks(int CardId)
        {
            var catalogCard = _context.CatalogCards.Find(CardId);
            return View(catalogCard);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooks(int CardId, int Count) 
        {
            for(int i = 0; i < Count; i++)
            {
                Book book = new Book();
                book.CatalogCardId = CardId;
                book.StatusId = 1;
                _context.Books.Add(book);
            }
            _context.SaveChangesAsync();
            return RedirectToAction("Details", "CatalogCard", new { id = CardId });
        }
    }
}
