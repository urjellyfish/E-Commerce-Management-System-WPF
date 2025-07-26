using E_CommerceManagementSystem.Business.Services;
using E_CommerceManagementSystem.Repository.Entities;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : UserControl
    {
        private OrderService _orderService = new();

        public OrderWindow()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var orders = _orderService.GetAll();
            OrderList.ItemsSource = orders;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();
            var result = _orderService.Search(keyword);
            OrderList.ItemsSource = result;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.Tag as Order;
            if (order != null)
            {
                _orderService.Update(order);
                MessageBox.Show($"Order {order.OrderID} updated to {order.Status}");
            }
        }

        //private void OrderList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    if (e.Column.Header.ToString() == "Status")
        //    {
        //        var editedOrder = e.Row.Item as Order;
        //        if (editedOrder != null)
        //        {
        //            _orderService.Update(editedOrder);
        //            MessageBox.Show($"Order {editedOrder.OrderID} updated to {editedOrder.Status}");
        //        }
        //    }
        //}
    }
}
