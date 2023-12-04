using HotelReservations.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelReservations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RoomsMI_Click(object sender, RoutedEventArgs e)
        {
            var roomsWindow = new Rooms();
            roomsWindow.Show();
        }

        private void UsersMI_Click(object sender, RoutedEventArgs e)
        {
            var usersWindow = new Users();
            usersWindow.Show();
        }

        private void PricelistMI_Click(object sender, RoutedEventArgs e)
        {
            var roomPricelist = new RoomPricelist();
            roomPricelist.Show();
        }

        private void ReservationMI_Click(object sender, RoutedEventArgs e)
        {
            var reservations = new Reservations();
            reservations.Show();
        }

        public void GuestsMI_Click(Object sender,  RoutedEventArgs e)
        {
            var guests = new Guests();
            guests.Show();
        }
    }
}
