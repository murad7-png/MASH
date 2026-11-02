using MASH.DAL;
using Microsoft.AspNetCore.Mvc;
using MASH.Models;
using Microsoft.AspNetCore.Authorization;

namespace MASH.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly DatabaseDAL _databaseDAL;
        public FavoritesController(DatabaseDAL databaseDAL, TmdbDAL tmdbDAL)
        {
            _databaseDAL = databaseDAL;
            _tmdbDAL = tmdbDAL;
        }
        private readonly TmdbDAL _tmdbDAL;

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var favorites = await _databaseDAL.GetFavorites(userId);
            foreach (var movie in favorites)
            {
                movie.PosterPath = await _tmdbDAL.GetMoviePoster(movie.Id);
            }
            return View(favorites);
        }
    }
}
