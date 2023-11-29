using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Model
{
    // Popričali smo o 4 različite varijante kako je moguće
    // implementirati korisnike. Odabrali smo ovu kako biste
    // probali nasleđivanje u C# i utvrdili polimorfizam. 
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string JMBG { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public UserType UserType { get; set; }
        public User Clone()
        {
            var clone  = new User(); 
            clone.Id = Id;
            clone.Name = Name;
            clone.Surname = Surname;
            clone.JMBG = JMBG;
            clone.Username = Username;
            clone.Password = Password;
            clone.UserType = UserType;
            clone.IsActive = IsActive;
            return clone;

        }
    }


}
