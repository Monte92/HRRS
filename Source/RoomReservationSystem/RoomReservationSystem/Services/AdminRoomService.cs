using RoomReservationSystem.Utilities;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Services
{
    public static class AdminRoomService
    {
        public static async Task ModifyRoom()
        {
            List<Room> rooms;

            try
            {
                rooms = await RoomService.GetAllRooms(DatabaseService.GetMariaDbConnection());
            }
            catch (Exception)
            {
                Console.WriteLine("System error: Could not retrieve rooms.");
                return;
            }

            MenuPrinter.PrintList(Room.ToOneLineString, rooms, true);

            var input = InputReader.ReadInt(1, rooms.Count, "Choose room to modify: ");
            var selectedRoom = rooms[input - 1];

            Console.WriteLine($"Modifying Room ID {selectedRoom.Id}...\n");

            while (true)
            {
                Console.WriteLine("Choose a modification:");
                var modifyMenuOptions = new List<string>
                {
                    "Room type",
                    "Pets allowed",
                    "Status",
                    "Exit modification menu"
                };

                MenuPrinter.PrintMenu(modifyMenuOptions, true);

                var modifyInput = InputReader.ReadInt(1, modifyMenuOptions.Count, "Enter an option number: ");

                if (modifyInput == 4)
                {
                    break;
                }

                switch (modifyInput)
                {
                    case 1:
                        Console.WriteLine("Modifying room type...\n");
                        Console.WriteLine("Select new room type:");

                        var roomTypes = Enum.GetNames<RoomType>().ToList();

                        MenuPrinter.PrintMenu(roomTypes, true);

                        var roomTypeInput = InputReader.ReadInt(1, roomTypes.Count, "Enter an option number: ");

                        selectedRoom.Type = (RoomType)roomTypeInput;

                        if (await UpdateRoom(selectedRoom))
                        {
                            Console.WriteLine($"Room {selectedRoom.Id} type changed to {selectedRoom.Type}.\n");
                        }

                        break;

                    case 2:
                        Console.WriteLine("Modifying pet policy...\n");
                        Console.WriteLine("Select new policy:");
                        
                        var petPolicies = new List<string>
                        {
                            "Allowed",
                            "Not Allowed"
                        };
                        
                        MenuPrinter.PrintMenu(petPolicies, true);
                        
                        var petTypeInput = InputReader.ReadInt(1, petPolicies.Count, "Enter an option number: ");
                        
                        if (petTypeInput == 1)
                        {
                            selectedRoom.PetsAllowed = true;
                        }
                        else
                        {
                            selectedRoom.PetsAllowed = false;
                        }

                        if (await UpdateRoom(selectedRoom))
                        {
                            Console.WriteLine(
                                $"Room {selectedRoom.Id} pet policy changed to {selectedRoom.PetsAllowed}.\n");
                        }

                        break;

                    case 3:
                        Console.WriteLine("Modifying status...\n");
                        Console.WriteLine("Set new status:");

                        var roomStatusOptions = Enum.GetNames<RoomStatus>().ToList();

                        MenuPrinter.PrintMenu(roomStatusOptions, true);

                        var statusTypeInput = InputReader.ReadInt(1, roomStatusOptions.Count);

                        selectedRoom.Status = (RoomStatus)(statusTypeInput);

                        if (await UpdateRoom(selectedRoom))
                        {
                            Console.WriteLine($"Room {selectedRoom.Id} status changed to {selectedRoom.Status}.\n");
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        public static void AddRoom()
        {
        }

        public static void DeleteRoom()
        {
        }

        private static async Task<bool> UpdateRoom(Room selectedRoom)
        {
            try
            {
                var updateResult =
                    await RoomService.UpdateRoom(DatabaseService.GetMariaDbConnection(), selectedRoom);

                if (!updateResult)
                {
                    Console.WriteLine("System error: Could not update room.");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("System error: Could not update room.");
                throw e;
                return false;
            }

            return true;
        }
    }
}