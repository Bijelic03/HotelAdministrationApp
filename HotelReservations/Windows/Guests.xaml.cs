using HotelReservations.Model;
using HotelReservations.Service;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addEditGuestWindow = new AddEditGuest();

            Hide();
            if (addEditGuestWindow.ShowDialog() == true)
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
