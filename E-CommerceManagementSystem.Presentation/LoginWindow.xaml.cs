using E_CommerceManagementSystem.Business.Services;
using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly CustomerService _service = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private bool Validate()
        {
            string patternEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (EmailTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Email is required!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (!Regex.IsMatch(EmailTextBox.Text.Trim(), patternEmail))
            {
                MessageBox.Show("Invalid email format!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (PasswordBox.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Password is required!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            if (!_service.IsAuth(EmailTextBox.Text.Trim(), PasswordBox.Password))
            {
                MessageBox.Show("Invalid email or password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Customer? customer = _service.GetCustomerByEmail(EmailTextBox.Text.Trim());

            if (customer != null)
            {
                UserWindow userWindow = new UserWindow();
                userWindow.Customer = customer;
                userWindow.Show();
            }



            Hide();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new();
            register.ShowDialog();
        }
    }
}
