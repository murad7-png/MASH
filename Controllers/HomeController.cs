using MASH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MASH.Services;
using Microsoft.AspNetCore.Authorization;

namespace MASH.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseService _databaseService;

        private readonly TmdbService _tmdbService;
        public HomeController(
            ILogger<HomeController> logger,
            TmdbService tmdbService,
            DatabaseService databaseService)
        {
            _logger = logger;
            _tmdbService = tmdbService;
            _databaseService = databaseService;
        }


        public async Task<IActionResult> Index()
        {

            var response = await _tmdbService.GetPopularMovies();
            return View(response.Results);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
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