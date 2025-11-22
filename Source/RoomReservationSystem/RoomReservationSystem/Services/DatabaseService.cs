using Microsoft.Data.Sqlite;
using MySqlConnector;

namespace RoomReservationSystem.Services
{
    public class DatabaseService
    {
        // Use IP if not using docker-compose to start both the db and app
        //private static string server = "db"; // 192.168.224.1
        private static string server = "192.168.224.1";

        private static string connection = $"Server={server};Port=3306;User ID=root;Password=test;Database=room_management_system";

        public SqliteConnection GetConnection()
        {
            using var connection = new SqliteConnection("Data Source=Data/room_reservation_system.db");

            return connection;
        }

        public static MySqlConnection GetMariaDbConnection()
        {
            // Handle proper disposing in caller
            return new MySqlConnection(connection);
        }
    }
}
