using HotelReservations.Exceptions;
using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private string ToCSV(RoomType roomType)
        {
            return $"{roomType.Id},{roomType.Name},{roomType.Value},{roomType.NightPrice}, {roomType.DayPrice},{roomType.IsActive}";
        }

        private RoomType FromCSV(string csv)
        {
            string[] csvValues = csv.Split(',');

            var roomType = new RoomType();
            roomType.Id = int.Parse(csvValues[0]);
            roomType.Name = csvValues[1];
            roomType.Value = int.Parse(csvValues[2]);
            roomType.NightPrice = double.Parse(csvValues[3]);
            roomType.DayPrice = double.Parse(csvValues[4]);
            roomType.IsActive = bool.Parse(csvValues[5]);

            return roomType;
        }

        public void Save(List<RoomType> roomTypeList)
        {
            try
            {
                using (var streamWriter = new StreamWriter("roomTypes.txt"))
                {
                    foreach (var roomType in roomTypeList)
                    {
                        streamWriter.WriteLine(ToCSV(roomType));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CouldntPersistDataException(ex.Message);
            }

        }

        public List<RoomType> Load()
        {
            if (!File.Exists("roomTypes.txt"))
            {
                return null;
            }

            try
            {
                using (var streamReader = new StreamReader("roomTypes.txt"))
                {
                    List<RoomType> roomTypes = new List<RoomType>();
                    string line;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var roomType = FromCSV(line);

                        roomTypes.Add(roomType);

                    }

                    return roomTypes;
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
