using HotelReservations.Exceptions;
using HotelReservations.Model;
using HotelReservations.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var loadedRoomTypes = roomTypeRepository.GetAll();

                if (loadedRoomTypes != null)
                {
                    Hotel.GetInstance().RoomTypes = loadedRoomTypes;
                }


                IGuestRepository guestRepository = new GuestRepository();
                var loadedGuests = guestRepository.GetAll();

                if (loadedGuests != null)
                {
                    Hotel.GetInstance().Guests = loadedGuests;
                }


                IRoomRepository roomRepository = new RoomRepository();
                var loadedRooms = roomRepository.GetAll();

                if (loadedRooms != null)
                {
                    Hotel.GetInstance().Rooms = loadedRooms;
                }



                IUserRepository userRepository = new UserRepository();
                var loadedUsers = userRepository.GetAll();

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


        
    }
}
