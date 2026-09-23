using System.Text.Json.Serialization;

namespace MASH.Models
{
    public class TmdbResponse
    {
        [JsonPropertyName("results")]
        public List<Movie> Results { get; set; }
    }
}