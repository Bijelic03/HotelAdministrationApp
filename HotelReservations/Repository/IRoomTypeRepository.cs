using HotelReservations.Model;
using System.Collections.Generic;

namespace HotelReservations.Repository
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetAll();
        int Insert(RoomType roomType);
        void Update(RoomType roomType);
        void Save(List<RoomType> roomTypeList);
    }
}
