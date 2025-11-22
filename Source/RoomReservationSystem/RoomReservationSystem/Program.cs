//using RoomReservationSystem.Handlers;


//MainMenuHandler.HandleNavigation();

using RoomReservationSystem.Models;
using RoomReservationSystem.Services;
using RoomReservationSystem.Utilities;

//Console.ReadLine();

Console.WriteLine("Welcome!");


var startDate = new DateOnly(2025,11,3);
var endDate = new DateOnly(2025,11,5);

var rooms = await RoomService.GetFreeAndAvailableRooms(DatabaseService.GetMariaDbConnection(), startDate, endDate);

MenuPrinter.PrintList(Room.ToOneLineString, rooms);


//await AdminRoomService.ModifyRoom();

// var room = new Room()
// {
//  PetsAllowed = true,
//  Type = RoomType.Double
// };
//
//
//
// room = await RoomService.CreateRoom(DatabaseService.GetMariaDbConnection(), room);
//
// room.PetsAllowed = false;
//
// var updated = await RoomService.UpdateRoom(DatabaseService.GetMariaDbConnection(), room);
//
// room = await RoomService.GetRoom(DatabaseService.GetMariaDbConnection(), room.Id);
//
// var removed = await RoomService.DeleteRoom(DatabaseService.GetMariaDbConnection(), room.Id);

/*
 Jos tässä kutsutaan suoraan db, niin voi olla, että
mariadb ei ole vielä toiminassa
kannataa siis laittaa vaikka alla oleva threadsleep
tai console.readline() pysäyttämään exceptionit
 */
//Thread.Sleep(3000);

// var rooms = await RoomService.GetAllRooms(DatabaseService.GetMariaDbConnection());
//
// foreach (var room in rooms)
// {
//     Console.WriteLine($"Room id: {room.Id}");
// }

Console.WriteLine("Bye!");