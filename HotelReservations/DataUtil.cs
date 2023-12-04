using HotelReservations.Exceptions;
using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations
{
    public class DataUtil
    {
        public static void LoadData()
        {
            Hotel hotel = Hotel.GetInstance();
            hotel.Id = 1;
            hotel.Name = "Hotel Park";
            hotel.Address = "Kod Futoskog parka...";




          //  hotel.RoomTypes.Add(singleBedRoom);
         //   hotel.RoomTypes.Add(doubleBedRoom);
         //  hotel.RoomTypes.Add(tripleBedRoom);
            

         


            // Može kada znamo da postoji rooms.txt datoteka
            // Ona bi trebalo da se nađe u potfolderu projektnog foldera
            // PopProjekat/bin/Debug

            try
            {


            }
               catch (CouldntLoadResourceException)
            {
                Console.WriteLine("Call an administrator, something weird is happening with the files");
            }
            catch (Exception ex)
            {
                Console.Write("An unexpected error occured", ex.Message);
            }


            try
            {

                IRoomTypeRepository roomTypeRepository = new RoomTypeRepository();
                var loadedRoomTypes = roomTypeRepository.Load();

                if (loadedRoomTypes != null)
                {
                    Hotel.GetInstance().RoomTypes = loadedRoomTypes;
                }


                IGuestRepository guestRepository = new GuestRepository();
                var loadedGuests = guestRepository.Load();

                if (loadedGuests != null)
                {
                    Hotel.GetInstance().Guests = loadedGuests;
                }


                IRoomRepository roomRepository = new RoomRepository();
                var loadedRooms = roomRepository.Load();

                if (loadedRooms != null)
                {
                    Hotel.GetInstance().Rooms = loadedRooms;
                }

                IUserRepository userRepository = new UserRepository();
                var loadedUsers = userRepository.Load();

                if (loadedUsers != null)
                {
                    Hotel.GetInstance().Users = loadedUsers;
                }

                IReservationRepository reservationRepository = new ReservationRepository();
                var loadedReservation = reservationRepository.Load();

                if (loadedReservation != null)
                {
                    Hotel.GetInstance().Reservations = loadedReservation;
                }





                // Samo za primer...
                //BinaryRoomRepository binaryRoomRepository = new BinaryRoomRepository();
                //var loadedRoomsFromBin = binaryRoomRepository.Load();
            }
            catch (CouldntLoadResourceException)
            {
                Console.WriteLine("Call an administrator, something weird is happening with the files");
            }
            catch (Exception ex)
            {
                Console.Write("An unexpected error occured", ex.Message);
            }
        }

        public static void PersistData()
        {
            try
            {
                // Kada se gasi program, čuvamo u rooms.txt
                // Posle toga će rooms.txt postojati (ako nešto ne pođe po zlu)
                IRoomRepository roomRepository = new RoomRepository();
                IRoomTypeRepository roomTypeRepository = new RoomTypeRepository();
                IUserRepository userRepository = new UserRepository();
                IReservationRepository reservationRepository = new ReservationRepository();
                IGuestRepository guestRepository = new GuestRepository();


                roomRepository.Save(Hotel.GetInstance().Rooms);
                roomTypeRepository.Save(Hotel.GetInstance().RoomTypes);
                userRepository.Save(Hotel.GetInstance().Users);
                reservationRepository.Save(Hotel.GetInstance().Reservations);
                guestRepository.Save(Hotel.GetInstance().Guests);
                //BinaryRoomRepository binaryRoomRepository = new BinaryRoomRepository();
                //binaryRoomRepository.Save(Hotel.GetInstance().Rooms);

            }
            catch (CouldntPersistDataException)
            {
                Console.WriteLine("Call an administrator, something weird is happening with the files");
            }
        }
    }
}
