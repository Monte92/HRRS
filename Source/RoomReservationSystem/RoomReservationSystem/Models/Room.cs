namespace RoomReservationSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }
        public RoomStatus Status { get; set; }
        public bool PetsAllowed { get; set; }
        public string Description { get; set; } = string.Empty;

        public override string ToString()
        {
            return
                $"""
                Room ID {Id}:
                Type: {Type}
                Status: {Status}
                Pets Allowed: {PetsAllowed}
                Description: {(string.IsNullOrEmpty(Description) ? "-" : Description)}
                """;
        }

        public static string ToOneLineString(Room room)
        {
            return $"Room ID {room.Id}: Type: {room.Type} | Pets Allowed: {room.PetsAllowed} | Status: {room.Status} | Description: {(string.IsNullOrEmpty(room.Description) ? "-" : room.Description)}";
        }
    }
}
