using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        private UserService userService;
        private ICollectionView view;

        public Users()
        {
            userService = new UserService();
            
            InitializeComponent();
            FillData();
        }

        // TODO: Korisničke lozinke ne bi trebalo prikazati
        private void FillData()
        {

            var users = userService.GetAllActiveUsers();

            view = CollectionViewSource.GetDefaultView(users);

            UsersDG.ItemsSource = null;
            UsersDG.ItemsSource = view;
            UsersDG.IsSynchronizedWithCurrentItem = true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addUsersWindow = new AddEditUser();

            Hide();
            if(addUsersWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = view.CurrentItem as User;

            if(selectedUser != null)
            {
                var editUsersWindow = new AddEditUser(selectedUser);
                editUsersWindow.ShowDialog();
                FillData();

                view.Refresh();

            }
        }

        private void UserIDSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            // Filter the view based on the entered User ID
            var userIdFilter = UserIDSearchTB.Text.Trim();
            if (!string.IsNullOrEmpty(userIdFilter))
            {
                view.Filter = item => ((User)item).Id.ToString().Contains(userIdFilter);
            }
            else
            {
                // If the filter is empty, clear the filter
                view.Filter = null;
            }
        }

        private void UsersDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedUser = view.CurrentItem as User;

            if (MessageBox.Show($"Are you sure that you want to delete user with id {selectedUser!.Id}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                userService.DeleteUser(selectedUser);
                FillData();
            }
            else
            {
            }
        }
    }

}
