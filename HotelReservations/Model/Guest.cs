using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using HotelReservations.Windows;

namespace HotelReservations.Model
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
        public int ReservationId { get; set; }


        public Guest Clone()
        {
            return new Guest
            {
                Id = Id,
                Name = Name,
                Surname = Surname,
                IDNumber = IDNumber,
                ReservationId = ReservationId,
            };
        }
    }
}
