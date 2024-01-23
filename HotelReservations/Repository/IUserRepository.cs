using HotelReservations.Model;
using System.Collections.Generic;

namespace HotelReservations.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();
        int Insert(User user);
        void Update(User user);
        void Save(List<User> userList);
    }
}
