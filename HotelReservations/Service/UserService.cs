
using HotelReservations.Model;
using HotelReservations.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Service
{
    public class UserService
    {
        public List<User> GetAllUsers()
        {
            return Hotel.GetInstance().Users;
        }

        public List<User> GetAllActiveUsers()
        {
            return Hotel.GetInstance().Users.Where(x => x.IsActive).ToList();
        }

        public User GetUserById(int id)
        {
            return Hotel.GetInstance().Users.FirstOrDefault(x => x.Id == id);
        }

        public void SaveUser(User user)
        {
            user.IsActive = true;
            if(user.Id == 0)
            {
                user.Id = GetNextIdValue();
                Hotel.GetInstance().Users.Add(user);

            }
            else
            {
                var index = Hotel.GetInstance().Users.FindIndex(r => r.Id == user.Id);
                Hotel.GetInstance().Users[index] = user;
            }
        }

        public void DeleteUser(User user)
        {
            var index = Hotel.GetInstance().Users.FindIndex(r => r.Id == user.Id);
            Hotel.GetInstance().Users[index].IsActive = false;
        }

        public int GetNextIdValue()
        {
            return Hotel.GetInstance().Users.Max(r => r.Id) + 1;
        }
    }
}
