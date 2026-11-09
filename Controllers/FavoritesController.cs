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
        // method to add a movie to favorites
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Add (int movieId)
        {
            var movie = await _tmdbDAL.GetMovie(movieId);
            if (movie == null)
            {
                return RedirectToAction ("Index","Home");
            }
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await _databaseDAL.SaveMovie(movie);
            await _databaseDAL.AddFavorite(userId, movieId);
            return RedirectToAction("Index", "Home");

        }
        // method to remove a movie from favorites
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int movieId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await _databaseDAL.RemoveFavorite(userId, movieId);
            return RedirectToAction("Index", "Home");

        }
    }
}
