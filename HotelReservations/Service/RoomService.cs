using HotelReservations.Model;
using HotelReservations.Repository;
using HotelReservations.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class RoomService
    {
        IRoomRepository roomRepository;
        public RoomService() 
        { 
            roomRepository = new RoomRepository();
        }

        public List<Room> GetAllRooms()
        {
            return Hotel.GetInstance().Rooms;
        }

        public List<Room> GetAllActiveRooms()
        {
            return Hotel.GetInstance().Rooms.Where(r => r.IsActive).ToList();
        }
        public List<Room> GetSortedRooms()
        {
            var rooms = Hotel.GetInstance().Rooms;
            rooms.Sort((r1, r2) => r1.RoomNumber.CompareTo(r2.RoomNumber));
            return rooms;
        }



        public List<Room> GetAllRoomsByRoomNumber(string startingWith)
        {
            var rooms = Hotel.GetInstance().Rooms;
            var filteredRooms = rooms.FindAll((r) => r.RoomNumber.StartsWith(startingWith));
            return filteredRooms;
        }

        public void SaveRoom(Room room)
        {
            room.IsActive = true;
            if (room.Id == 0)
            {
                
                room.Id = GetNextIdValue();
                Hotel.GetInstance().Rooms.Add(room);
            }
            else
            {
                var index = Hotel.GetInstance().Rooms.FindIndex(r => r.Id == room.Id);
                Hotel.GetInstance().Rooms[index] = room;
            }
        }

         public void DeleteRoom(Room room)
        {
            var index = Hotel.GetInstance().Rooms.FindIndex(r => r.Id == room.Id);
            Hotel.GetInstance().Rooms[index].IsActive = false;
        }




        public int GetNextIdValue()
        {
            if (Hotel.GetInstance().Rooms.Count == 0)
            {
                return 1;
            }
            else
            {
                return Hotel.GetInstance().Rooms.Max(r => r.Id) + 1;
            }
        }
    }
}
