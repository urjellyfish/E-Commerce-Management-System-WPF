﻿using E_CommerceManagementSystem.Business.Services;
using E_CommerceManagementSystem.Repository.Entities;
using E_CommerceManagementSystem.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private ObservableCollection<Product> cartItems = new();
        private OrderService orderService = new();
        public Customer Customer { get; set; }

        private ProductService _productService = new();
        private CategoryService _categoryService = new();
        public List<Order>? Orders { get; set; }
        public Order SelectedOrder { get; set; }
        public List<Product> OrderProducts { get; set; }
        public UserWindow()
        {
            InitializeComponent();
            CartList.ItemsSource = cartItems;
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            DisplayCustomerInfo();
            LoadOrderDetail();
            if (SelectedOrder != null)
            {
                LoadProductsForSelectedOrder();
            }

        }

        private void DisplayCustomerInfo()
        {
            if (Customer != null)
            {
                ProfileCustomerIdText.Text = Customer.CustomerID.ToString();
                ProfileCustomerNameText.Text = Customer.Name;
                ProfileCustomerEmailText.Text = Customer.Email;
            }
        }

        private void LoadData()
        {
            ProductList.ItemsSource = null;
            ProductList.ItemsSource = _productService.GetAll();
            
            CbCategoryFilter.ItemsSource = null;
            CbCategoryFilter.ItemsSource = _categoryService.GetAll();
            CbCategoryFilter.SelectedIndex = -1; // Không chọn gì khi load
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                LoadData();
            }

            var result = _productService.Search(txtKeyword.Text.Trim());
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

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product selectedProduct)
            {
                if (!cartItems.Any(p => p.ProductID == selectedProduct.ProductID))
                {
                    cartItems.Add(selectedProduct);
                    MessageBox.Show("Add successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("This product is already in the cart.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                cartItems.Remove(product);
            }
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }

            var result = MessageBox.Show("Are you sure to place the order?", "Confirm",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var order = new Order
                {
                    OrderAmount = cartItems.Sum(p => p.Price),
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    CustomerID = Customer.CustomerID,
                };
                
                orderService.Create(order);

                foreach (var item in cartItems)
                {
                    var product = _productService.GetProductById(item.ProductID);
                    if (product != null)
                    {
                        product.OrderID = order.OrderID;
                        _productService.Update(product);
                    }
                }

               
                MessageBox.Show("Order successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                cartItems.Clear();
                LoadData();
                LoadOrderDetail();
                SelectedOrder = order;
                LoadProductsForSelectedOrder();
            }
        }


        private void CbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCategoryFilter.SelectedValue != null)
            {
                int selectedCategory = (int)CbCategoryFilter.SelectedValue;
                ProductList.ItemsSource = _productService.FilterByCate(selectedCategory);
            }
            else
            {
                ProductList.ItemsSource = _productService.GetAll();
            }
        }

        private void LoadOrderDetail()
        {
            Orders = orderService.GetOrdersByCustomerId(Customer.CustomerID);

            if (Orders != null && Orders.Any())
            {
                OrderList.ItemsSource = null;
                OrderList.ItemsSource = Orders;
                OrderList.SelectedIndex = 0;
            }
        }

        private void OrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderList.SelectedItem is Order selected)
            {
                // Update thông tin chi tiết
                CustomerNameText.Text = selected.Customer?.Name ?? "";
                OrderDateText.Text = selected.OrderDate.ToString("yyyy-MM-dd");
                TotalAmountText.Text = selected.OrderAmount.ToString("C");
                StatusText.Text = selected.Status.ToString();

                // Load sản phẩm
                var productList = _productService.GetAllProductByOrderId(selected.OrderID);
                OrderProductList.ItemsSource = null;
                OrderProductList.ItemsSource = productList;
            }
        }

        private void LoadProductsForSelectedOrder()
        {
            if (SelectedOrder == null) return;
            var productList = _productService.GetAllProductByOrderId(SelectedOrder.OrderID);
            OrderProductList.ItemsSource = null;
            OrderProductList.ItemsSource = productList;
        }


        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}