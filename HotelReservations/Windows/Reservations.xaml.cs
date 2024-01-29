using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HotelReservations.Model;
using HotelReservations.Service;

namespace HotelReservations.Windows
{
    public partial class Reservations : Window
    {
        private ReservationService reservationService;
        private ICollectionView view;
        private ICollectionView viewCurrent;

        private DateTime? startDate;
        private DateTime? endDate;
        public Reservations()
        {
            InitializeComponent();
            reservationService = new ReservationService();

            // Onemogući automatsko generisanje kolona
            ReservationsDG.AutoGenerateColumns = false;
            CurrentReservationsDG.AutoGenerateColumns = false;

            FillData();
        }

        public void FillData()
        {
            var allReservations = reservationService.GetAllActiveReservations();

            // Set the filter for view to DoFilter method
            view = CollectionViewSource.GetDefaultView(allReservations);
            view.Filter = DoFilter;

            // Filter reservations that are currently ongoing
            var currentReservations = allReservations.Where(r => r.StartDateTime <= DateTime.Now && r.EndDateTime >= DateTime.Now).ToList();

            // Set the filter for viewCurrent to null (no additional filtering)
            viewCurrent = CollectionViewSource.GetDefaultView(currentReservations);
            viewCurrent.Filter = null;

            ReservationsDG.ItemsSource = null;
            ReservationsDG.ItemsSource = view;
            ReservationsDG.IsSynchronizedWithCurrentItem = true;

            CurrentReservationsDG.ItemsSource = null;
            CurrentReservationsDG.ItemsSource = viewCurrent;
            CurrentReservationsDG.IsSynchronizedWithCurrentItem = true;
        }






        private bool DoFilter(object reservationObject)
        {
            var reservation = reservationObject as Reservation;

            var roomNumberSearchParam = RoomNumberSearchTB.Text;

            startDate = StartDateTimePicker.SelectedDate;
            endDate = EndDateTimePicker.SelectedDate;

            bool roomNumberMatch = string.IsNullOrWhiteSpace(roomNumberSearchParam) || reservation.Room.RoomNumber.Contains(roomNumberSearchParam);
            bool startDateMatch = !startDate.HasValue || reservation.StartDateTime.Date >= startDate.Value.Date;
            bool endDateMatch = !endDate.HasValue || reservation.EndDateTime.Date <= endDate.Value.Date;

            return roomNumberMatch && startDateMatch && endDateMatch;
        }



        private void StartDateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            startDate = StartDateTimePicker.SelectedDate;

            if (startDate.HasValue && EndDateTimePicker.SelectedDate.HasValue && startDate > EndDateTimePicker.SelectedDate)
            {
                MessageBox.Show("End date cannot be before the start date.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                StartDateTimePicker.SelectedDate = null;
            }
            view.Refresh();

        }

        private void EndDateTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDate = EndDateTimePicker.SelectedDate;

            if (endDate.HasValue && StartDateTimePicker.SelectedDate.HasValue && endDate < StartDateTimePicker.SelectedDate)
            {
                MessageBox.Show("Start date cannot be after the end date.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                EndDateTimePicker.SelectedDate = null;
            }
            view.Refresh();

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addReservationWindow = new AddEditReservation();

            Hide();
            if (addReservationWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedReservation = (Reservation)view.CurrentItem;

            if (selectedReservation != null)
            {
                var editReservationWindow = new AddEditReservation(selectedReservation);

                Hide();

                if (editReservationWindow.ShowDialog() == true)
                {
                    FillData();
                }

                Show();
            }
        }

        private void RoomNumberSearchTB_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Refresh();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (view.CurrentItem == null) { return; }

            var selectedReservation = view.CurrentItem as Reservation;

            if (MessageBox.Show($"Are you sure that you want to delete reservation {selectedReservation!.Id}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                reservationService.DeleteReservation(selectedReservation);
                FillData();
            }
        }
    }
}
