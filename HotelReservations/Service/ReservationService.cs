using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace HotelReservations.Service
{
    public class ReservationService
    {
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

        public void SaveReservation(Reservation reservation)
        {
            reservation.IsActive = true;
            if (reservation.Id == 0)
            {
                reservation.Id = GetNextIdValue();
                foreach(Guest guest in reservation.Guests)
                {
                    Debug.WriteLine(guest.Name);

                }
              

                Hotel.GetInstance().Reservations.Add(reservation);
            }
            else
            {
                var index = Hotel.GetInstance().Reservations.FindIndex(r => r.Id == reservation.Id);
                Hotel.GetInstance().Reservations[index] = reservation;
            }
        }

        public void DeleteReservation(Reservation reservation)
        {
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
