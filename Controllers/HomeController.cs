using DarkLibCW.Areas.Identity.Data;
using DarkLibCW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DarkLibCW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<DarkLibUser> _userManager;
        private readonly AppDBContext _context;
        public HomeController(ILogger<HomeController> logger, UserManager<DarkLibUser> userManager, AppDBContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Ваш код для получения Id записи пользователя по его имени пользователя
            string userName = User.Identity.Name;
            int? subscriberId = await GetSubscriberIdByUserName(userName);

            ViewBag.SubscriberId = subscriberId;

            return View();
        }

        private async Task<int?> GetSubscriberIdByUserName(string userName)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.UserName == userName);
            return subscriber?.Id;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}