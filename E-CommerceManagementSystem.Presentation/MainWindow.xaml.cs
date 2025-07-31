using System.Windows;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _currentUserEmail;

        public MainWindow(string userEmail)
        {
            InitializeComponent();
            _currentUserEmail = userEmail;
        }

        private void CheckUserRole()
        {            
            if (_currentUserEmail == "admin@FUMiniTikiSystem.com")
            {
                CategoryManagement.Visibility = Visibility.Visible;
            }
        }        
    }
}