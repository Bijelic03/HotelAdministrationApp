using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class RoomTypeService
    {

        public List<RoomType> GetAllRoomTypes()
        {
            return Hotel.GetInstance().RoomTypes;
        }

        public List<RoomType> GetAllActiveRoomTypes()
        {
            return Hotel.GetInstance().RoomTypes.Where(x => x.IsActive).ToList();
        }

        public void SaveRoomType(RoomType roomType)
        {

            roomType.IsActive = true;

            if (roomType.Id == 0)
            {
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
            var index = Hotel.GetInstance().RoomTypes.FindIndex(r => r.Id == roomType.Id);
            Hotel.GetInstance().RoomTypes[index].IsActive = false;
        }

        public int GetNextIdValue()
        {
            return Hotel.GetInstance().RoomTypes.Max(r => r.Id) + 1;
        }
    }
}
