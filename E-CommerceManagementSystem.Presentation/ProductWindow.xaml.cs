﻿using System;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class ProductWindow : UserControl
    {
        private ProductService _service = new();
        private CategoryService _categoryService = new();
        private LMStudioService _testGenerator = new();
        public ProductWindow()
        {
            InitializeComponent();
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            ProductList.ItemsSource = null;
            ProductList.ItemsSource = _service.GetAll();

            CbCategoryFilter.ItemsSource = null;
            CbCategoryFilter.ItemsSource = _categoryService.GetAll();
            CbCategoryFilter.SelectedIndex = -1; // Không chọn gì khi load
        }
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            ProductDetailWindow d = new ProductDetailWindow();
            d.ShowDialog();
            LoadData();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Product selected = (Product)ProductList.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Please choose a product form the list below");
                return;
            }

            ProductDetailWindow d = new ProductDetailWindow();
            d.Edited = selected;
            d.ShowDialog();
            LoadData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Product selected = (Product)ProductList.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Please choose a product form the list below");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete product {selected.ProductID} ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _service.Delete(selected);
                LoadData();
            }
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

        private void CbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCategoryFilter.SelectedValue != null)
            {
                int selectedCategory = (int)CbCategoryFilter.SelectedValue;
                ProductList.ItemsSource = _service.FilterByCate(selectedCategory);
            }
            else
            {
                // Nếu không chọn gì, hiển thị tất cả sản phẩm
                ProductList.ItemsSource = _service.GetAll();
            }
        }

        private async void BtnGenerateTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _testGenerator.GenerateCodeAsync();
                MessageBox.Show("Unit test generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating unit test: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
