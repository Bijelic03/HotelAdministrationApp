using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    internal class RoomPricelistService
    {
        public List<RoomType> GetAllRoomTypes()
        {
            return Hotel.GetInstance().RoomTypes;
        }

        public void SaveRoomType(RoomType roomType)
        {
            if(roomType.Id  == 0)
            {
                roomType = new RoomType();
                roomType.Id = GetNextIdValue();
                Hotel.GetInstance().RoomTypes.Add(roomType);

            }
            else
            {
                var index = Hotel.GetInstance().RoomTypes.FindIndex(r => r.Id == roomType.Id);
                Hotel.GetInstance().RoomTypes[index] = roomType;
            }
        }

        public void DeleteRoomType(RoomType roomType)
        {
           var index = Hotel.GetInstance().RoomTypes.FindIndex(r => r.Id ==roomType.Id);
           
        }

        public int GetNextIdValue()
        {
            return Hotel.GetInstance().Rooms.Max(r => r.Id) + 1;
        }

    }
}
