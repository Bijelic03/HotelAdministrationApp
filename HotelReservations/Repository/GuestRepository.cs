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

namespace HotelReservations.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private string ToCSV(Guest guest)
        {
            //,{guest.ReservationId}
            return $"{guest.Id},{guest.Name},{guest.Surname},{guest.IDNumber}";
        }

        private Guest FromCSV(string csv)
        {
            string[] csvValues = csv.Split(',');

            var guest = new Guest();
            guest.Id = int.Parse(csvValues[0]);
            guest.Name = csvValues[1];
            guest.Surname = csvValues[2];
            guest.IDNumber = csvValues[3];
        //    guest.ReservationId = int.Parse(csvValues[4]);

            return guest;
        }

        public void Save(List<Guest> guestList)
        {
            try
            {
                using (var streamWriter = new StreamWriter("guests.txt"))
                {
                    foreach (var guest in guestList)
                    {
                        streamWriter.WriteLine(ToCSV(guest));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }
        }

        public List<Guest> Load()
        {
            if (!File.Exists("guests.txt"))
            {
                return null;
            }

            try
            {
                using (var streamReader = new StreamReader("guests.txt"))
                {
                    List<Guest> guests = new List<Guest>();
                    string line;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var guest = FromCSV(line);
                        guests.Add(guest);
                    }

                    return guests;
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
