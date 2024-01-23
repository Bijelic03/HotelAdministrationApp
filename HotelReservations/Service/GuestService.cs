using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservations.Repository;

namespace HotelReservations.Service
{
    public class GuestService
    {
        private IGuestRepository guestRepository;

        public GuestService() {
            guestRepository = new GuestRepository();
        }
        public List<Guest> GetAllGuests()
        {
            return Hotel.GetInstance().Guests;
        }

        public List<Guest> GetGuestsWIthoutRoom()
        {
            return Hotel.GetInstance().Guests.Where(g => g.ReservationId == 0).ToList();
        }



        public Guest GetGuestById(int id)
        {
            return Hotel.GetInstance().Guests.FirstOrDefault(x => x.Id == id);
        }



        public void SaveGuest(Guest guest)
        {
            if (guest.Id == 0)
            {
                guest.Id = guestRepository.Insert(guest);
                Hotel.GetInstance().Guests.Add(guest);
            }
            else
            {
                guestRepository.Update(guest);
                var index = Hotel.GetInstance().Guests.FindIndex(r => r.Id == guest.Id);
                Hotel.GetInstance().Guests[index] = guest;
            }
        }







        public int GetNextIdValue()
        {
            if (Hotel.GetInstance().Guests.Count == 0)
            {
                return 1;
            }
            else
            {
                return Hotel.GetInstance().Guests.Max(r => r.Id) + 1;
            }
        }
    }
}
