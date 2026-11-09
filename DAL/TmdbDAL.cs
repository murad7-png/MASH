using MASH.Models;
using System.Linq.Expressions;
using System.Text.Json;
namespace MASH.DAL
{
    public class TmdbDAL
    {
        private readonly HttpClient _httpClient;

        public TmdbDAL(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            var token = configuration["TMDB:AccessToken"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<TmdbResponse> GetPopularMovies()
        {
            var response = await _httpClient.GetAsync("movie/popular");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TmdbResponse>(json);
        }
        // method to retrive movie poster path by movie ID for favorites page
        public async Task<string?> GetMoviePoster(int movieId)
        {

            try
            {

                var response = await _httpClient.GetAsync($"movie/{movieId}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                var movie = JsonSerializer.Deserialize<Movie>(json);
                return movie?.PosterPath;
            }
            catch (Exception)
            { return null; }
        }

        // method to retrive movie by movie ID for favorites page
        public async Task<Movie?> GetMovie(int movieId)
        {

            try
            {

                var response = await _httpClient.GetAsync($"movie/{movieId}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                var movie = JsonSerializer.Deserialize<Movie>(json);
                return movie;
            }
            catch (Exception)
            { return null; }
        }
    }
} 