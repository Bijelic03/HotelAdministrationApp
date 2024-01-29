using HotelReservations.Model;
using System.Collections.Generic;

namespace HotelReservations.Repository
{
    public interface IReservationRepository
    {
        List<Reservation> GetAll();
        int Insert(Reservation reservation);
        void Update(Reservation reservation);
    }
}
