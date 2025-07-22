using E_CommerceManagementSystem.Business.Services.CustomerServ;
using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly ICustomerService _service;

        public RegisterWindow(ICustomerService service)
        {
            InitializeComponent();

            _service = service;
        }

        private bool Validate()
        {
            string patternName = @"^[A-Z][a-zA-Z\s]+$"; // Must start with a capital letter, only letters and spaces allowed
            string patternEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (NameTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Name is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (!Regex.IsMatch(NameTextBox.Text.Trim(), patternName))
            {
                MessageBox.Show("Name must start with a capital letter and can only contain letters and spaces.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (EmailTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Email is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (!Regex.IsMatch(EmailTextBox.Text.Trim(), patternEmail))
            {
                MessageBox.Show("Invalid email format.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (PasswordBox.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Password is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            Customer customer = new()
            {
                Name = NameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password.Trim()
            };

            _service.Create(customer);
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.IsNullOrEmpty() && EmailTextBox.Text.IsNullOrEmpty() && PasswordBox.Password.IsNullOrEmpty())
            {
                Close();
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you really want to close?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Close();
        }
    }
}
