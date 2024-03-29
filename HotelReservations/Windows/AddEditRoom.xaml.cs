﻿using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelReservations.Windows
{
    /// <summary>
    /// Interaction logic for AddEditRoom.xaml
    /// </summary>
    public partial class AddEditRoom : Window
    {
        private RoomService roomService;
        private RoomTypeService roomTypeService;
        private Room contextRoom;
        public AddEditRoom(Room? room = null)
        {
            if(room == null)
            {
                contextRoom = new Room();
            }
            else
            {
                contextRoom = room.Clone();
            }
                      
            InitializeComponent();
            roomTypeService = new RoomTypeService();
            roomService = new RoomService();

            AdjustWindow(room);

            this.DataContext = contextRoom;
        }

        public void AdjustWindow(Room? room = null)
        {
            if (room != null)
            {
                Title = "Edit Room";
            }
            else
            {
                Title = "Add Room";
            }

            var roomTypes = roomTypeService.GetAllActiveRoomTypes();
            RoomTypesCB.ItemsSource = roomTypes;



        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            roomService.SaveRoom(contextRoom);

            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
