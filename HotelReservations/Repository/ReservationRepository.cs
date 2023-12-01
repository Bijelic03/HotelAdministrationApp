using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HotelReservations.Exceptions;
using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.IO;

using HotelReservations.Exceptions;
using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace HotelReservations.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private string ToCSV(Reservation reservation)
        {
          
            string guestsCSV = string.Join(";", reservation.Guests); 

            return $"{reservation.Id},{(int)reservation.ReservationType},{reservation.StartDateTime},{reservation.EndDateTime},{reservation.TotalPrice},{guestsCSV}";
        }

        private Reservation FromCSV(string csv)
        {
            string[] csvValues = csv.Split(',');

            var reservation = new Reservation();
            reservation.Id = int.Parse(csvValues[0]);
            reservation.ReservationType = (ReservationType)int.Parse(csvValues[1]);
            reservation.StartDateTime = DateTime.Parse(csvValues[2]);
            reservation.EndDateTime = DateTime.Parse(csvValues[3]);
            reservation.TotalPrice = double.Parse(csvValues[4]);

           
            string guestsCSV = csvValues[5];
            reservation.Guests = guestsCSV.Split(';')
                               .Select(guestData => new Guest { Name = guestData })  
                               .ToList();

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
