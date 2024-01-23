using HotelReservations.Model;
using HotelReservations.Service;
using System.ComponentModel;
using System.Configuration;
using System.Windows;

namespace HotelReservations.Windows
{
    public partial class Login : Window, INotifyPropertyChanged
    {
        private UserService userService;

        public Login()
        {
            userService = new UserService();
            InitializeComponent();
            DataContext = this;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (userService.Login(username, password))
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show( "Wrong password or username.");

                }
            }
            else
            {
                MessageBox.Show("Please enter both username and password.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
