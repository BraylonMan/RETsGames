using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RETsGames.Data;
using RETsGames.Models;
using System.Diagnostics;

namespace RETsTickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly RETsGamesContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, RETsGamesContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var games = await _context.Game.ToListAsync();
            return View(games);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}

