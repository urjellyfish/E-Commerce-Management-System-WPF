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
using System.Windows.Navigation;
using System.Windows.Shapes;
using E_CommerceManagementSystem.Business.DTO;
using E_CommerceManagementSystem.Business.Services;
using Microsoft.IdentityModel.Tokens;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for CategoryManagementWindow.xaml
    /// </summary>
    public partial class CategoryManagementWindow : UserControl
    {
        private readonly CategoryService _categoryService;
        public CategoryManagementWindow()
        {
            InitializeComponent();
            _categoryService = new CategoryService();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetAll();
            CategoryDataGrid.ItemsSource = categories;
        }

        private void CategoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryDataGrid.SelectedItem is CategoryDTO selectedCategory)
            {
                CategoryIdTextBox.Text = selectedCategory.CategoryId.ToString();
                NameTextBox.Text = selectedCategory.Name;
                DescriptionTextBox.Text = selectedCategory.Description;
            }
        }

        private bool Validate()
        {
            if (NameTextBox.Text.IsNullOrEmpty() || DescriptionTextBox.Text.IsNullOrEmpty())
                return false;

            //foreach (var cat in _categoryService.GetAll())
            //    if ()

            return true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                MessageBox.Show("Please fill required field!", "Validate", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 

            var newCategory = new CategoryDTO
            {
                Name = NameTextBox.Text,
                Picture = "",
                Description = DescriptionTextBox.Text
            };

            _categoryService.AddCategory(newCategory);
            MessageBox.Show("Category added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCategories();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                MessageBox.Show("Please fill required field!", "Validate", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a category to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedCategory = new CategoryDTO
            {
                CategoryId = int.Parse(CategoryIdTextBox.Text),
                Name = NameTextBox.Text,
                Picture = "",
                Description = DescriptionTextBox.Text
            };

            _categoryService.UpdateCategory(updatedCategory);
            MessageBox.Show("Category updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCategories();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryDataGrid.SelectedItem is not CategoryDTO selectedCategory)
            {
                MessageBox.Show("Please select a category to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedCategory.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _categoryService.DeleteCategory(selectedCategory.CategoryId);
                MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
            }
        }
    }
}
