using HotelReservations.Service;
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

    public partial class MainWindow : Window
    {
       // private Visibility IsAdmin;
       // private Visibility IsReceptionist;

        public MainWindow()
        {
          // IsAdmin = UserService.LoggedUser.UserType == Model.UserType.ADMIN ? Visibility.Visible : Visibility.Collapsed;
          //  IsReceptionist = UserService.LoggedUser.UserType == Model.UserType.RECEPTIONIST ? Visibility.Visible : Visibility.Collapsed;

            InitializeComponent();
        }


     


        private void RoomsMI_Click(object sender, RoutedEventArgs e)
        {
            if(UserService.LoggedUser.UserType == Model.UserType.ADMIN) {
                var roomsWindow = new Rooms();
                roomsWindow.Show();
            }
            else
            {
                MessageBox.Show("You do not have permission to access this feature.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UsersMI_Click(object sender, RoutedEventArgs e)
        {

            if (UserService.LoggedUser.UserType == Model.UserType.ADMIN)
            {
                var usersWindow = new Users();
                usersWindow.Show();
            }
            else
            {
                MessageBox.Show("You do not have permission to access this feature.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void LogoutMI_Click(Object sender, RoutedEventArgs e)
        {
            UserService.LoggedUser = null;
            var loginWindow = new Login();
            loginWindow.Show();
            this.Close();


        }

        private void PricelistMI_Click(object sender, RoutedEventArgs e)
        {


            if (UserService.LoggedUser.UserType == Model.UserType.ADMIN)
            {
                var roomPricelist = new RoomPricelist();
                roomPricelist.Show();
            }
            else
            {
                MessageBox.Show("You do not have permission to access this feature.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void ReservationMI_Click(object sender, RoutedEventArgs e)
        {
            if (UserService.LoggedUser.UserType == Model.UserType.RECEPTIONIST)
            {
                var reservations = new Reservations();
                reservations.Show();
            }
            else
            {
                MessageBox.Show("You do not have permission to access this feature.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
  
        }

        public void GuestsMI_Click(Object sender,  RoutedEventArgs e)
        {

            if (UserService.LoggedUser.UserType == Model.UserType.RECEPTIONIST)
            {
                var guests = new Guests();
                guests.Show();
            }
            else
            {
                MessageBox.Show("You do not have permission to access this feature.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
   
        }
    }
}
