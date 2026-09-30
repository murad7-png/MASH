using Microsoft.Data.SqlClient;
using MASH.Models;

namespace MASH.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
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
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
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
    }
}