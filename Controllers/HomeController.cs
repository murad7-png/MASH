using MASH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MASH.DAL;
using Microsoft.AspNetCore.Authorization;

namespace MASH.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly TmdbDAL _tmdbDAL;
        private readonly DatabaseDAL _databaseDAL;
        public HomeController(
            ILogger<HomeController> logger,
            TmdbDAL tmdbDAL,
            DatabaseDAL databaseDAL)
        {
            _logger = logger;
            _tmdbDAL = tmdbDAL;
            _databaseDAL = databaseDAL;
        }


        public async Task<IActionResult> Index()
        {

            var response = await _tmdbDAL.GetPopularMovies();
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            ViewBag.FavoriteIds = await _databaseDAL.GetFavoritesIds(userId);
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
            return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}