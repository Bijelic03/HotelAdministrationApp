﻿using HotelReservations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservations.Repository
{
    public interface IGuestRepository
    {
        List<Guest> Load();
        void Save(List<Guest> guestList);
    }
}