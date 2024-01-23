using HotelReservations.Model;
using System.Collections.Generic;

namespace HotelReservations.Repository
{
    public interface IGuestRepository
    {
        List<Guest> GetAll();
        int Insert(Guest guest);
        void Update(Guest guest);
        void Save(List<Guest> guestList);
    }
}
