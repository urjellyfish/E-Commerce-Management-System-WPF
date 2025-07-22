using E_CommerceManagementSystem.Business.Services.CustomerServ;
using System.Windows;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ICustomerService _service;
        private readonly RegisterWindow _registerWindow;

        public LoginWindow(ICustomerService service, RegisterWindow registerWindow)
        {
            InitializeComponent();

            _service = service;
            _registerWindow = registerWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_service.IsAuth(EmailTextBox.Text.Trim(), PasswordBox.Password))
            {
                MessageBox.Show("Invalid email or password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow main = new();
            main.Show();
            Hide();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _registerWindow.ShowDialog();
        }
    }
}
