using Dapper;
using MySqlConnector;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Services
{
    public static class RoomService
    {
        /// <summary>
        /// Asynchronously retrieves a list of all rooms from the database.
        /// </summary>
        /// <param name="connection">The MySQL connection to the database.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a list of <see cref="Room"/> instances.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the connection is null.</exception>
        /// <exception cref="MySqlException">Thrown when there is an error executing the SQL command.</exception>
        public static async Task<List<Room>> GetAllRooms(MySqlConnection connection)
        {
            await using (connection)
            {
                var query = "SELECT * FROM room";

                var rooms = await connection.QueryAsync<Room>(query);

                return rooms.ToList();
            }
        }

        /// <summary>
        /// Asynchronously retrieves a room from the database based on the specified ID.
        /// </summary>
        /// <param name="connection">The MySQL connection to the database.</param>
        /// <param name="id">The unique identifier of the room to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the <see cref="Room"/> instance if found; otherwise, returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the connection is null.</exception>
        /// <exception cref="MySqlException">Thrown when there is an error executing the SQL command.</exception>
        public static async Task<Room?> GetRoom(MySqlConnection connection, int id)
        {
            await using (connection)
            {
                var query = "SELECT * FROM room WHERE id = @id";

                var room = await connection.QueryFirstOrDefaultAsync<Room>(query, new { id });

                return room;
            }
        }

        /// <summary>
        /// Asynchronously creates a new room in the database and returns the created room instance.
        /// </summary>
        /// <param name="connection">The MySQL connection to the database.</param>
        /// <param name="room">An instance of the <see cref="Room"/> class containing the details of the room to be created.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the created <see cref="Room"/> instance with its ID populated.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the connection is null or the room object is null.</exception>
        /// <exception cref="MySqlException">Thrown when there is an error executing the SQL command.</exception>
        public static async Task<Room> CreateRoom(MySqlConnection connection, Room room)
        {
            await using (connection)
            {
                var query = """
                            INSERT INTO room (type, status, pets_allowed)
                            VALUES (@Type, @Status, @PetsAllowed);
                            SELECT LAST_INSERT_ID();
                            """;

                var result = await connection.ExecuteScalarAsync<int>(query, room);

                room.Id = result;

                return room;
            }
        }

        /// <summary>
        /// Asynchronously updates the details of a room in the database.
        /// </summary>
        /// <param name="connection">The MySQL connection to the database.</param>
        /// <param name="room">An instance of the <see cref="Room"/> class containing the updated room details.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a boolean indicating whether the update was successful.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the connection is null or the room object is null.</exception>
        /// <exception cref="MySqlException">Thrown when there is an error executing the SQL command.</exception>
        public static async Task<bool> UpdateRoom(MySqlConnection connection, Room room)
        {
            await using (connection)
            {
                var query = """
                            UPDATE room
                            SET
                            type = @Type,
                            status = @Status,
                            pets_allowed = @PetsAllowed
                            WHERE id = @Id;
                            """;

                var result = await connection.ExecuteAsync(query, room);

                return result > 0;
            }
        }

        /// <summary>
        /// Asynchronously deletes a room from the database based on the specified ID.
        /// </summary>
        /// <param name="connection">The MySQL connection to the database.</param>
        /// <param name="id">The unique identifier of the room to be deleted.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a boolean indicating whether the deletion was successful.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the connection is null.</exception>
        /// <exception cref="MySqlException">Thrown when there is an error executing the SQL command.</exception>
        public static async Task<bool> DeleteRoom(MySqlConnection connection, int id)
        {
            await using (connection)
            {
                var query = "DELETE FROM room WHERE id = @id";

                var result = await connection.ExecuteAsync(query, new { id });

                return result > 0;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of available rooms that are not reserved during the specified date range.
        /// </summary>
        /// <param name="connection">The MySQL connection to the database.</param>
        /// <param name="startDate">The start date of the reservation period.</param>
        /// <param name="endDate">The end date of the reservation period.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a list of <see cref="Room"/> instances that are free and available.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when argument is null.</exception>
        /// <exception cref="ArgumentException">Thrown when startDate is after endDate.</exception>
        /// <exception cref="MySqlException">Thrown when there is an error executing the SQL command.</exception>
        public static async Task<List<Room>> GetFreeAndAvailableRooms(MySqlConnection connection, DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before end date");
            }
            
            await using (connection)
            {
                var query =
                    """
                    SELECT
                        *
                    FROM
                        room
                    WHERE
                        room.status = 'Available' AND room.id NOT IN (
                        SELECT
                            room_id
                        FROM
                            reservation
                        WHERE
                            @startDate <= reservation.end_date AND @endDate >= reservation.start_date
                    );        
                    """;
                
                var rooms = await connection.QueryAsync<Room>(query, new { startDate = startDate.ToDateTime(TimeOnly.MinValue), endDate =  endDate.ToDateTime(TimeOnly.MinValue) });

                return rooms.ToList();
            }
        }
    }
}