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
        private IRoomRepository roomRepository;
        private ReservationService reservationService;
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

        public List<Room> GetAllFreeRooms()
        {
            reservationService = new ReservationService();

            List<Room> rooms = new List<Room>();
            foreach (Room room in GetAllActiveRooms())
            {

                if (!reservationService.GetAllActiveReservations().Exists(reservation => reservation.Room == room))
                {
                    rooms.Add(room);
                }
            }
            foreach (Reservation reservation in reservationService.GetAllActiveReservations() )
            {
                if (!GetAllActiveRooms().Contains(reservation.Room))
                {
                    rooms.Add(reservation.Room);
                }
            }
            return rooms;
        }

        public List<Room> GetAllOcupiedRooms()
        {
            List<Room> rooms = new List<Room>();
            foreach (Reservation reservation in Hotel.GetInstance().Reservations)
            {
                if (GetAllActiveRooms().Contains(reservation.Room))
                { 
                    rooms.Add(reservation.Room);
                }
            }
            return rooms;
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
            if (room.Id == 0)
            {
                room.Id = roomRepository.Insert(room);
                Hotel.GetInstance().Rooms.Add(room);
            }
            else
            {
                roomRepository.Update(room);
                var index = Hotel.GetInstance().Rooms.FindIndex(r => r.Id == room.Id);
                Hotel.GetInstance().Rooms[index] = room;
            }
        }




        public void DeleteRoom(Room room)
        {
            room.IsActive = false;
            roomRepository.Update(room);
            var index = Hotel.GetInstance().Rooms.FindIndex(r => r.Id == room.Id);
            Hotel.GetInstance().Rooms[index].IsActive = false;
        }

        public Room getRoomById(int id)
        {
            return Hotel.GetInstance().Rooms.Find(r => r.Id == id);

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
