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

        private RoomService roomService;
        private ICollectionView view;
        private ReservationService reservationService;
        private GuestService guestService;
        private Reservation contextReservation;
        private DateTime? startDate;
        private DateTime? endDate;
        List<Guest> oldGuests = null;

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
            roomService = new RoomService();
            guestService = new GuestService();
            reservationService = new ReservationService();
            AdjustWindow(reservation);
            FillData();

            this.DataContext = contextReservation;
        }

        private void StartDateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            startDate = StartDateTimePicker.SelectedDate;

            if (startDate.HasValue && EndDateTimePicker.SelectedDate.HasValue && startDate > EndDateTimePicker.SelectedDate)
            {
                MessageBox.Show("End date cannot be before the start date.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                StartDateTimePicker.SelectedDate = null;
            }
        }

        private void EndDateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDate = EndDateTimePicker.SelectedDate;

            if (endDate.HasValue && StartDateTimePicker.SelectedDate.HasValue && endDate < StartDateTimePicker.SelectedDate)
            {
                MessageBox.Show("Start date cannot be after the end date.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                EndDateTimePicker.SelectedDate = null;
            }
        }


        private void AdjustWindow(Reservation reservation = null)
        {
            ReservationTypesCB.ItemsSource = Enum.GetValues(typeof(ReservationType));
            RoomTypesCB.ItemsSource = roomService.GetAllActiveRooms();
            if (reservation != null)
            {
                Title = "Edit reservation";

                oldGuests = reservation.Guests;
                RoomTypesCB.SelectedItem = reservation.Room;
                RoomTypesCB.IsEnabled = true;

                StartDateTimePicker.IsEnabled = true;
                StartDateTimePicker.SelectedDate = reservation.StartDateTime;
                EndDateTimePicker.IsEnabled = true;
                EndDateTimePicker.SelectedDate = reservation.EndDateTime;
                ReservationTypesCB.SelectedItem = reservation.ReservationType;
                ReservationTypesCB.IsEnabled = true;

                foreach (var item in GuestsSelectDG.Items)
                {
                    var guest = item as Guest;

                    if (guest != null)
                    {
                        Debug.WriteLine($"Checking guest: {guest.Id}");

                        if (reservation.Guests.Contains(guest))
                        {
                            var cellContent = GuestsSelectDG.Columns[0].GetCellContent(item);
                            if (cellContent is CheckBox checkBox)
                            {
                                checkBox.IsChecked = true;
                                Debug.WriteLine("Checkbox checked!");
                            }
                        }
                    }
                }

            }
            else
            {
                Title = "Add reservation";
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            contextReservation.ReservationType = (ReservationType) ReservationTypesCB.SelectedItem;
            contextReservation.Guests = SelectedGuests();

            if (RoomTypesCB.SelectedItem == null || ReservationTypesCB.SelectedItem == null || contextReservation.Guests.Count == 0 || startDate == null || endDate == null)
            {
                MessageBox.Show("Fill all the fields", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                contextReservation.StartDateTime = (DateTime) StartDateTimePicker.SelectedDate;
                contextReservation.EndDateTime = (DateTime)EndDateTimePicker.SelectedDate;

                contextReservation.Room = (Room)RoomTypesCB.SelectedItem;
                int numberOfDays = (int)(endDate - startDate).Value.TotalDays;
                if (contextReservation.Guests.Count > contextReservation.Room.RoomType.Value)
                {
                    MessageBox.Show("You have too many guests for this room", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }

                double price = 0;
                if (contextReservation.ReservationType == ReservationType.Day)
                {
                     price = contextReservation.Room.RoomType.DayPrice * contextReservation.Guests.Count * numberOfDays;

                }
                else
                {
                    price = (double)contextReservation.Room.RoomType.NightPrice * contextReservation.Guests.Count * numberOfDays;

                }

                MessageBoxResult result = MessageBox.Show("Price is: " + price + " Are you sure you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    contextReservation.TotalPrice = price;
                    reservationService.SaveReservation(contextReservation, oldGuests);
                    DialogResult = true;
                    Close();
                }

            }
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





        private List<Guest> SelectedGuests()
        {
            var selectedGuests = new List<Guest>();

            foreach (var item in GuestsSelectDG.Items)
            {
                var guest = item as Guest;

         
                if (guest != null)
                {
                    var cellContent = GuestsSelectDG.Columns[0].GetCellContent(item);
                    if (cellContent is CheckBox checkBox)
                    {
                        bool isChecked = checkBox.IsChecked ?? false;

                        if (isChecked)
                        {
                            selectedGuests.Add(guest);
                        }
                    }
                }
            }

            return selectedGuests;
        }




        private void FillData()
        {
            var guestsWithoutRoom = guestService.GetGuestsWIthoutRoom();

            GuestsSelectDG.Columns.Clear();

            DataGridCheckBoxColumn checkBoxColumn = new DataGridCheckBoxColumn();
            checkBoxColumn.Header = "Select";
            checkBoxColumn.Binding = new Binding("IsSelected");

            GuestsSelectDG.Columns.Add(checkBoxColumn);

            GuestsSelectDG.ItemsSource = null;
            GuestsSelectDG.ItemsSource = guestsWithoutRoom;
            GuestsSelectDG.IsSynchronizedWithCurrentItem = true;
        }




    }


}
