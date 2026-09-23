using MASH.Models;
using System.Text.Json;
namespace MASH.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
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
    }
}