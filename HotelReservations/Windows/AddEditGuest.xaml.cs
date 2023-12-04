using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;

namespace HotelReservations.Windows
{
    public partial class AddEditGuest : Window
    {
        private GuestService guestService;
        private Guest contextGuest;

        public AddEditGuest(Guest guest = null)
        {
            if (guest == null)
            {
                contextGuest = new Guest();
            }
            else
            {
                contextGuest = guest.Clone();
            }

            InitializeComponent();
            guestService = new GuestService();
            AdjustWindow(guest);
            this.DataContext = contextGuest;
        }

        private void AdjustWindow(Guest guest = null)
        {
            // Adjust window properties based on whether it's an edit or add operation.
            if (guest != null)
            {
                Title = "Edit Guest";
            }
            else
            {
                Title = "Add Guest";
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            guestService.SaveGuest(contextGuest);
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
