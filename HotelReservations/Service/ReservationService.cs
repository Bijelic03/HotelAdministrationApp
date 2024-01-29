using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HotelReservations.Model;

using System.Diagnostics;
using HotelReservations.Repository;
using HotelReservations.Windows;

namespace HotelReservations.Service
{
    public class ReservationService
    {

        RoomService roomService;
        IReservationRepository reservationRepository;
        IGuestRepository guestRepository;
        public ReservationService()
        {
            roomService = new RoomService();
            guestRepository = new GuestRepository();
            reservationRepository = new ReservationRepository();
        }
        public List<Reservation> GetAllReservations()
        {
            return Hotel.GetInstance().Reservations;
        }

        public List<Reservation> GetAllActiveReservations()
        {
            return Hotel.GetInstance().Reservations.Where(x => x.IsActive).ToList();
        }


        public Reservation GetReservationById(int id)
        {

            return Hotel.GetInstance().Reservations.FirstOrDefault(x => x.Id == id);
        }

        public void SaveReservation(Reservation reservation, List<Guest> oldGuests)
        {
            reservation.IsActive = true;

            if (reservation.Id == 0)
            {
                reservation.Id = reservationRepository.Insert(reservation);
                Hotel.GetInstance().Reservations.Add(reservation);
                if(oldGuests != null) 
                {
                    foreach (Guest guest in oldGuests)
                    {

                            guest.ReservationId = 0;
                            var index = Hotel.GetInstance().Guests.FindIndex(r => r.Id == guest.Id);
                            Hotel.GetInstance().Guests[index] = guest;
                            guestRepository.Update(guest);
                    
                    }
                }
                foreach (Guest guest in reservation.Guests)
                {
                    guest.ReservationId = reservation.Id;
                    var index = Hotel.GetInstance().Guests.FindIndex(r => r.Id == guest.Id);
                    Hotel.GetInstance().Guests[index] = guest;
                    guestRepository.Update(guest);

                }
            }
            else
            {
                reservationRepository.Update(reservation);
                var reservationIndex = Hotel.GetInstance().Reservations.FindIndex(r => r.Id == reservation.Id);
                Hotel.GetInstance().Reservations[reservationIndex] = reservation;
                if (oldGuests != null)
                {
                    foreach (Guest guest in oldGuests)
                    {

                        guest.ReservationId = 0;
                        var index = Hotel.GetInstance().Guests.FindIndex(r => r.Id == guest.Id);
                        Hotel.GetInstance().Guests[index] = guest;
                        guestRepository.Update(guest);

                    }
                }
                foreach (Guest guest in reservation.Guests)
                {
                    guest.ReservationId = reservation.Id;
                    var index = Hotel.GetInstance().Guests.FindIndex(r => r.Id == guest.Id);
                    Hotel.GetInstance().Guests[index] = guest;
                    guestRepository.Update(guest);

                }
            }
        }


        public void DeleteReservation(Reservation reservation)
        {
            reservation.IsActive = false;
            reservationRepository.Update(reservation);
            var index = Hotel.GetInstance().Reservations.FindIndex(r => r.Id == reservation.Id);
            Hotel.GetInstance().Reservations[index].IsActive = false;
        }

        public int GetNextIdValue()
        {
            if (Hotel.GetInstance().Reservations.Count == 0)
            {
                return 1;
            }
            else
            {
                return Hotel.GetInstance().Reservations.Max(r => r.Id) + 1;
            }
        }
    }
}
