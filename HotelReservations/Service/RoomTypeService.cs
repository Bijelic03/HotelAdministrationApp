using HotelReservations.Model;
using HotelReservations.Repository;
using HotelReservations.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class RoomTypeService
    {
        private IRoomTypeRepository roomTypeRepository ;
        public RoomTypeService() {
            roomTypeRepository = new RoomTypeRepository();
        }
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
                roomType.Id = roomTypeRepository.Insert(roomType);
                Hotel.GetInstance().RoomTypes.Add(roomType);
            }
            else
            {
                roomTypeRepository.Update(roomType);
                var index = Hotel.GetInstance().RoomTypes.FindIndex(r => r.Id == roomType.Id);
                Hotel.GetInstance().RoomTypes[index] = roomType;
            }
        }
        public bool isRoomTypeUsed(RoomType roomType)
        {
            return Hotel.GetInstance().Rooms.Exists(r => r.RoomType == roomType);

        }
        public bool DeleteRoomType(RoomType roomType)
        {
            if (!isRoomTypeUsed(roomType))
            {
                roomType.IsActive = false;
                roomTypeRepository.Update(roomType);
                var index = Hotel.GetInstance().RoomTypes.FindIndex(r => r.Id == roomType.Id);
                Hotel.GetInstance().RoomTypes[index].IsActive = false;
                return true;
            }
            else { return false; }
        }



        public int GetNextIdValue()
        {
            return Hotel.GetInstance().RoomTypes.Max(r => r.Id) + 1;
        }
    }
}
