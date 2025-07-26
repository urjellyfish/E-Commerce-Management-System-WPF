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
using E_CommerceManagementSystem.Business.Services;
using E_CommerceManagementSystem.Repository.Entities;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private ProductService _service = new();

        public UserWindow()
        {
            InitializeComponent();
        }





            private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                LoadData();
            }

            private void LoadData()
            {
                ProductList.ItemsSource = null;
                ProductList.ItemsSource = _service.GetAll();
            }

            private void BtnSearch_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtKeyword.Text))
                {
                    LoadData();
                }

                var result = _service.Search(txtKeyword.Text.Trim());
                ProductList.ItemsSource = null;

                if (result.Count > 0)
                {
                    ProductList.ItemsSource = result;
                }
                else
                {
                    MessageBox.Show("Not found");
                    return;
                }
            }



        }
    }

