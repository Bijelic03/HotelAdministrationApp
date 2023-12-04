using HotelReservations.Model;
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

using HotelReservations.Model;
using HotelReservations.Service;
using System;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;

namespace HotelReservations.Windows
{

    public partial class AddEditReservation : Window
    {

        private ICollectionView view;
        private ReservationService reservationService;
        private GuestService guestService;
        private Reservation contextReservation;

        public AddEditReservation(Reservation reservation = null)
        {
            if (reservation == null)
            {
                contextReservation = new Reservation();
            }
            else
            {
                contextReservation = reservation.Clone();
            }
            InitializeComponent();
            guestService = new GuestService();
            reservationService = new ReservationService();
            AdjustWindow(reservation);
            FillData();

            this.DataContext = contextReservation;
        }

        private void AdjustWindow(Reservation reservation = null)
        {
            ReservationTypesCB.ItemsSource = Enum.GetValues(typeof(ReservationType));

            if (reservation != null)
            {
                Title = "Edit reservation";

                ReservationTypesCB.SelectedItem = reservation.ReservationType;
                ReservationTypesCB.IsEnabled = true;
            }
            else
            {
                Title = "Add reservation";
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (contextReservation.TotalPrice <= 0)
            {
                MessageBox.Show("Total price must be greater than 0.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            contextReservation.Guests = SelectedGuests();
            reservationService.SaveReservation(contextReservation);
            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            var addEditGuest = new AddEditGuest();
            addEditGuest.Show();
        }


        private Dictionary<Guest, bool> selectedGuestsDict = new Dictionary<Guest, bool>();

        private List<Guest> SelectedGuests()
        {
            var selectedGuests = new List<Guest>();

            foreach (var item in GuestsSelectDG.Items)
            {
                var guest = item as Guest;

                // Ako gost nije null, proveri da li je označen
                if (guest != null)
                {
                    var cellContent = GuestsSelectDG.Columns[0].GetCellContent(item);
                    if (cellContent is CheckBox checkBox)
                    {
                        bool isChecked = checkBox.IsChecked ?? false;

                        if (isChecked)
                        {
                            Debug.WriteLine("checkovan je");
                            selectedGuests.Add(guest);
                        }
                    }
                }
            }

            return selectedGuests;
        }




        private void FillData()
        {
            var freeGuests = guestService.GetAllFreeGuests();

            GuestsSelectDG.Columns.Clear();

            // Dodajte CheckBoxColumn programski
            DataGridCheckBoxColumn checkBoxColumn = new DataGridCheckBoxColumn();
            checkBoxColumn.Header = "Select";
            checkBoxColumn.Binding = new Binding("IsSelected");

            GuestsSelectDG.Columns.Add(checkBoxColumn);

            GuestsSelectDG.ItemsSource = null;
            GuestsSelectDG.ItemsSource = freeGuests;
            GuestsSelectDG.IsSynchronizedWithCurrentItem = true;
        }




    }


}
