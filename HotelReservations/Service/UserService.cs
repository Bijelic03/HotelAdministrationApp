
using HotelReservations.Model;
using HotelReservations.Repository;
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
        private UserRepository userRepository;
        public UserService() { 
        userRepository = new UserRepository();
        }
        public List<User> GetAllUsers()
        {
            return Hotel.GetInstance().Users;
        }

        public List<User> GetAllActiveUsers()
        {
            return Hotel.GetInstance().Users.Where(x => x.IsActive).ToList();
        }

        public List<User> GetAllActiveGuests()
        {
            return Hotel.GetInstance().Users.Where(x => x.UserType == UserType.GUEST).ToList();
        }

        public List<User> GetAllGuestsInRoom()
        {
            return Hotel.GetInstance().Users.Where(x => x.UserType == UserType.GUEST).ToList();
        }
        public User GetUserById(int id)
        {
            return Hotel.GetInstance().Users.FirstOrDefault(x => x.Id == id);
        }

        public Boolean Login(string username, string password)
        {
            User user = userRepository.Login(username, password);
            return user != null;
        }

        public void SaveUser(User user)
        {
            user.IsActive = true;
            if(user.Id == 0)
            {
                user.Id = userRepository.Insert(user);
                Hotel.GetInstance().Users.Add(user);

            }
            else
            {
                userRepository.Update(user);
                var index = Hotel.GetInstance().Users.FindIndex(r => r.Id == user.Id);
                Hotel.GetInstance().Users[index] = user;

            }
        }

        public void DeleteUser(User user)
        {
            user.IsActive = false;
            userRepository.Update(user);
            var index = Hotel.GetInstance().Users.FindIndex(r => r.Id == user.Id);
            Hotel.GetInstance().Users[index].IsActive = false;
        }

        public int GetNextIdValue()
        {
            if (Hotel.GetInstance().Users.Count == 0)
            {
                return 1;
            }
            else
            {
                return Hotel.GetInstance().Users.Max(r => r.Id) + 1;
            }
        }
    }
}
