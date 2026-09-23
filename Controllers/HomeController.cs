using MASH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MASH.Services;

namespace MASH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly TmdbService _tmdbService;
        public HomeController(ILogger<HomeController> logger, TmdbService tmdbService)
                    {
            _logger = logger;
            _tmdbService = tmdbService;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
