using HotelReservations.Exceptions;
using HotelReservations.Model;
using HotelReservations.Service;
using HotelReservations.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public class ReservationRepository : IReservationRepository
    {

        private GuestService guestService;

        private string ToCSV(Reservation reservation)
        {
            string guestsCSV = string.Join(";", reservation.Guests?.Select(g => g.Id) ?? Enumerable.Empty<int>());
            return $"{reservation.Id},{reservation.ReservationType},{guestsCSV},{reservation.StartDateTime},{reservation.EndDateTime},{reservation.TotalPrice},{reservation.IsActive}";
        }

        private Reservation FromCSV(string csv)
        {
            guestService = new GuestService();

            string[] csvValues = csv.Split(',');

            var reservation = new Reservation();
            reservation.Id = int.Parse(csvValues[0]);
            reservation.ReservationType = (ReservationType)Enum.Parse(typeof(ReservationType), csvValues[1], true);
            string guestsCSV = csvValues[2];
            List<int> guestsId = guestsCSV.Split(';')
                                           .Select(s => int.Parse(s))
                                           .ToList();

            List<Guest> guests = new List<Guest>();

            foreach(int id in guestsId)
            {
                guests.Add(guestService.GetGuestById(id));
            }
            reservation.Guests = guests;

            reservation.StartDateTime = DateTime.Parse(csvValues[3]);
            reservation.EndDateTime = DateTime.Parse(csvValues[4]);
            reservation.TotalPrice = double.Parse(csvValues[5]);
            reservation.IsActive = bool.Parse(csvValues[6]);

            return reservation;
        }

        public void Save(List<Reservation> reservationList)
        {
            try
            {
                using (var streamWriter = new StreamWriter("reservations.txt"))
                {
                    foreach (var reservation in reservationList)
                    {
                        streamWriter.WriteLine(ToCSV(reservation));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }

        }

        public List<Reservation> Load()
        {
            if (!File.Exists("reservations.txt"))
            {
                return null;
            }

            try
            {
                using (var streamReader = new StreamReader("reservations.txt"))
                {
                    List<Reservation> reservations = new List<Reservation>();
                    string line;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var reservation = FromCSV(line);

                        reservations.Add(reservation);
                    }

                    return reservations;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new CouldntLoadResourceException(ex.Message);
            }
        }
    }
}
