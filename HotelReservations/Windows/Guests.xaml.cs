using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace HotelReservations.Windows
{
    public partial class Guests : Window
    {
        private GuestService guestService;
        private ICollectionView view;

        public Guests()
        {
            guestService = new GuestService();

            InitializeComponent();
            FillData();
        }

        private void FillData()
        {
            var guests = guestService.GetAllGuests();

            view = CollectionViewSource.GetDefaultView(guests);

            GuestsDG.ItemsSource = null;
            GuestsDG.ItemsSource = view;
            GuestsDG.IsSynchronizedWithCurrentItem = true;
        }

        private void GuestNameSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Filter = DoNameFilter;
            view.Refresh();
        }

        private bool DoNameFilter(object guestObject)
        {
            var guest = guestObject as Guest;

            if (guest != null && !string.IsNullOrEmpty(GuestNameSearchTB.Text))
            {
                // Implement your filter logic based on guest name
                return guest.Name.Contains(GuestNameSearchTB.Text, StringComparison.OrdinalIgnoreCase) ||
                       guest.Surname.Contains(GuestNameSearchTB.Text, StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addGuestWindow = new AddEditGuest();

            Hide();
            if (addGuestWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedGuest = view.CurrentItem as Guest;

            if (selectedGuest != null)
            {
                var editGuestWindow = new AddEditGuest(selectedGuest);
                editGuestWindow.ShowDialog();

                view.Refresh();
            }
            FillData();
        }

        private void GuestsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedGuest = view.CurrentItem as Guest;

            if (MessageBox.Show($"Are you sure that you want to delete guest with id {selectedGuest!.Id}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                FillData();
            }
            else
            {
                // Handle the case where the user chooses not to delete the guest.
            }
        }
    }
}