using Microsoft.Data.SqlClient;
using MASH.Models;

namespace MASH.DAL
{
    public class DatabaseDAL
    {
        private readonly string _connectionString;

        public DatabaseDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetUser(string username)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string query = @"
                SELECT UserId, Username, Email, PasswordHash, Role
                FROM Users
                WHERE Username = @Username";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Username", username);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    Role = reader.GetString(4)
                };
            }

            return null;
        }
        public async Task<List<User>> GetUsers()
        {
            List<User> users = new List<User>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string query = @"
        SELECT UserId, Username, Email, PasswordHash, Role
        FROM Users";

            using SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    Role = reader.GetString(4)
                });
            }

            return users;
        }

        public async Task TestConnection()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();
        }

        public async Task AddFavorite(int userId, int movieId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string query = @"
                INSERT INTO Favorites (UserId, MovieId)
                SELECT @UserId, @MovieId
                WHERE NOT EXISTS (
                    SELECT 1 FROM Favorites
                    WHERE UserId = @UserId AND MovieId = @MovieId
                )";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@MovieId", movieId);
            await command.ExecuteNonQueryAsync();
        }
        public async Task SaveMovie(Movie movie)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Movies WHERE MovieId = @MovieId)
                BEGIN
                    INSERT INTO Movies (MovieId, Title, Overview, ReleaseDate, VoteAverage)
                    VALUES (@MovieId, @Title, @Overview, @ReleaseDate, @VoteAverage)
                END";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MovieId", movie.Id);
            command.Parameters.AddWithValue("@Title", movie.Title);
            command.Parameters.AddWithValue("@Overview", movie.Overview ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@VoteAverage", movie.VoteAverage);
            await command.ExecuteNonQueryAsync();
        }
        public async Task <List<Movie>> GetFavorites(int userid)
        {
            List<Movie> favorites = new List<Movie>();
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string query = @"
                SELECT m.MovieId, m.Title, m.Overview, m.ReleaseDate, m.VoteAverage
                FROM Favorites f
                JOIN Movies m ON f.MovieId = m.MovieId
                WHERE f.UserId = @UserId";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userid);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                favorites.Add(new Movie
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Overview = reader.IsDBNull(2) ? null : reader.GetString(2),
                    ReleaseDate = reader.IsDBNull(3) ? null : reader.GetString(3),
                    VoteAverage = reader.GetDouble(4)
                });
            }
            return favorites;
        }
    }
}