using E_CommerceManagementSystem.Business.Services;
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
        private readonly CustomerService _service = new();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private bool Validate()
        {
            string patternName = @"^([A-Z][a-zA-Z]*)+( [A-Z][a-zA-Z]*)*$"; // Each word must start with a capital letter, only letters and spaces allowed
            string patternEmail = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Name không được để rỗng
            if (NameTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Name is required!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // Name phải chữ hoa từng đầu từ
            if (!Regex.IsMatch(NameTextBox.Text.Trim(), patternName))
            {
                MessageBox.Show("Each word must start with a capital letter and only letters and spaces are allowed!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // Email không được để trống
            if (EmailTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Email is required!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // Email phải chuẩn form
            if (!Regex.IsMatch(EmailTextBox.Text.Trim(), patternEmail))
            {
                MessageBox.Show("Invalid email format!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // Password không được để trống
            if (PasswordBox.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Password is required!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // PasswordAgain không được để trống
            if (PasswordBoxAgain.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Password again is required!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // PasswordAgain phải khớp với Password
            if (PasswordBox.Password != PasswordBoxAgain.Password)
            {
                MessageBox.Show("Passwords do not match!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            // Check email không được trùng
            if (_service.GetCustomerByEmail(EmailTextBox.Text.Trim()) != null)
            {
                MessageBox.Show("This email is already registered!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            MessageBox.Show("Registration successful! Please login to continue!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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

            // Đã nhập trường thì khi Close sẽ confirm lại
            MessageBoxResult result = MessageBox.Show("Do you really want to close?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Close();
        }
    }
}
