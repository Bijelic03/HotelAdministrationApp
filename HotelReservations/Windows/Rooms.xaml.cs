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
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class Rooms : Window
    {
        private RoomService roomService;
        private RoomTypeService roomTypeService;

        private ICollectionView view;
        public Rooms()
        {
            InitializeComponent();
            roomTypeService = new RoomTypeService();
            roomService = new RoomService();
            DataContext = roomService;
            FillData();
            RoomTypeFilterCB.ItemsSource = roomTypeService.GetAllActiveRoomTypes();

        }

        public void FillData()
        {
            var roomService = new RoomService();
            var rooms = roomService.GetAllActiveRooms();

            view = CollectionViewSource.GetDefaultView(rooms);
            view.Filter = DoFilter;

            RoomsDG.ItemsSource = null;
            RoomsDG.ItemsSource = view;
            RoomsDG.IsSynchronizedWithCurrentItem = true;
        }
        private bool DoFilter(object roomObject)
        {
            var room = roomObject as Room;

            var roomNumberSearchParam = RoomNumberSearchTB.Text;
            var roomTypeFilter = GetSelectedRoomTypeFilter();
            var isOccupiedFilter = GetSelectedAvailabilityFilter();

            // Retrieve the list of free or occupied rooms based on the filter.
            List<Room> filteredRooms;
            if (isOccupiedFilter.HasValue && isOccupiedFilter.Value)
            {
                filteredRooms = roomService.GetAllOcupiedRooms();
            }
            else if (isOccupiedFilter.HasValue && !isOccupiedFilter.Value)
            {
                filteredRooms = roomService.GetAllFreeRooms();
            }
            else
            {
                // No filter applied for availability status.
                filteredRooms = roomService.GetAllActiveRooms();
            }

            // Apply filters based on room number, room type, and availability status.
            return room.RoomNumber.Contains(roomNumberSearchParam) &&
                   (roomTypeFilter == null || room.RoomType == roomTypeFilter) &&
                   filteredRooms.Contains(room);
        }


        private bool? GetSelectedAvailabilityFilter()
        {
            if (AvailabilityFilterCB.SelectedIndex >= 0)
            {
                switch (AvailabilityFilterCB.SelectedIndex)
                {
                    case 0: // All
                        return null;
                    case 1: // Free
                        return false;
                    case 2: // Occupied
                        return true;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }



        private void RoomTypeFilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view.Refresh();
        }

        private void AvailabilityFilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (view != null)
            {
                view.Refresh();
            }
        }

        private RoomType GetSelectedRoomTypeFilter()
        {
            var selectedItem = RoomTypeFilterCB.SelectedItem as RoomType;
            return selectedItem;
        }



        private void RoomsDG_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.PropertyName.ToLower() == "IsActive".ToLower())
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addRoomWindow = new AddEditRoom();

            Hide();
            if(addRoomWindow.ShowDialog() == true)
            {
                FillData();
            }
            Show();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = (Room) view.CurrentItem;

            if(selectedRoom != null)
            {
                var editRoomWindow = new AddEditRoom(selectedRoom);
                
                Hide();

                if (editRoomWindow.ShowDialog() == true)
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

        // TODO: Završi započeto
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(view.CurrentItem == null) { return; }

            var selectedRoom = view.CurrentItem as Room;

            if (MessageBox.Show($"Are you sure that you want to delete room {selectedRoom!.RoomNumber}?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                roomService.DeleteRoom(selectedRoom);
                FillData();
            }
            else
            {
            }
        }
    }
}
