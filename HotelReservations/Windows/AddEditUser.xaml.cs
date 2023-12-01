using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AddEditUser.xaml
    /// </summary>
    public partial class AddEditUser : Window
    {
        private UserService userService;

        private User contextUser;
        public AddEditUser(User user = null)
        {
            if (user == null)
            {
                contextUser = new User();
            }
            else
            {
                contextUser = user.Clone();
            }

            InitializeComponent();
            userService = new UserService();
            AdjustWindow(user);

            // Set DataContext here
            this.DataContext = contextUser;
        }

        private void AdjustWindow(User user = null)
        {
            // Inicijalizujte ComboBox koristeći generičku kolekciju
            UserTypesCB.ItemsSource = Enum.GetValues(typeof(UserType));

            if (user != null)
            {
                Title = "Edit user";

                // Postavite izabranu vrednost ComboBox-a na tip korisnika
                UserTypesCB.SelectedItem = user.UserType;
                UserTypesCB.IsEnabled = true;
            }
            else
            {
                Title = "Add user";
            }
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            userService.SaveUser(contextUser);

            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(Object sender, RoutedEventArgs e)
        {

        }
    }
}
