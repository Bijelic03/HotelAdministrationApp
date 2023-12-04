using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace HotelReservations.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public ReservationType ReservationType { get; set; }
        public List<Guest> Guests { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double TotalPrice { get; set; }
        public bool IsActive { get; set; }

        public string GuestsIds => string.Join(", ", Guests.Select(g => g.Id));

        public Reservation Clone()
        {
            return new Reservation
            {
                Id = Id,
                ReservationType = ReservationType,
                Guests = CloneGuests(Guests),
                StartDateTime = StartDateTime,
                EndDateTime = EndDateTime,
                TotalPrice = TotalPrice,
                IsActive = IsActive
            };
        }

        private List<Guest> CloneGuests(List<Guest> guests)
        {
            var clonedGuests = new List<Guest>();

            foreach (var guest in guests)
            {
                clonedGuests.Add(guest.Clone());
            }

            return clonedGuests;
        }
    }
}
