using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Model
{
    [Serializable]
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Value { get; set; }
        public bool IsActive { get; set; }

        public double NightPrice { get; set; }

        public double DayPrice { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public RoomType Clone()
        {
            var clone = new RoomType();
            clone.Id = Id;
            clone.Name = Name;
            clone.Value = Value;
            clone.IsActive = IsActive;
            clone.NightPrice = NightPrice;
            clone.DayPrice = DayPrice;
            return clone;
        }
    }
}
