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
using Microsoft.IdentityModel.Tokens;

namespace E_CommerceManagementSystem.Presentation
{
    /// <summary>
    /// Interaction logic for ProductDetailWindow.xaml
    /// </summary>
    public partial class ProductDetailWindow : Window
    {
        public Product Edited { get; set; }
        private ProductService _service = new();
        private CategoryService _cateService = new();
        public ProductDetailWindow()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategory();
            FillLable();
            FillElements(Edited);
        }

        private void LoadCategory()
        {
            CbCategoryName.ItemsSource = _cateService.GetAll();
        }

        private void FillLable()
        {
            if (Edited == null)
            {
                WindowMode.Text = "Add a new Product";
                //txtProductID.Text = (_service.GetMaxId() + 1).ToString();
                //txtProductID.IsEnabled = false;
            }
            else
            {
                WindowMode.Text = "Update a Product";
                //txtProductID.Text = Edited.ProductID.ToString();
                //txtProductID.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Product p = new();

            //Validate inputs
            if (string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    CbCategoryName.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButton.OK , MessageBoxImage.Exclamation);
                return;
            }

            if(txtName.Text.Trim().Length < 5 || txtName.Text.Trim().Length > 100)
            {
                MessageBox.Show("Product name must be between 5 and 100 characters.", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txtDescription.Text.Trim().Length < 5 || txtDescription.Text.Trim().Length > 100)
            {
                MessageBox.Show("Description name must be between 5 and 100 characters.", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txtName.Text.Trim().Any(c => "@$#&*".Contains(c)))
            {
                MessageBox.Show("Product name cannot contain special characters such as @$#&*", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txtDescription.Text.Trim().Any(c => "@$#&*".Contains(c)))
            {
                MessageBox.Show("Description cannot contain special characters such as @$#&*", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal budget) || budget <= 0)
            {
                MessageBox.Show("Price must be a valid positive number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            p.Name = txtName.Text;
            p.Description = txtDescription.Text;
            p.Price = decimal.Parse(txtPrice.Text);
            if (CbCategoryName.SelectedValue != null)
                p.CategoryID = (int)CbCategoryName.SelectedValue;

            //ktra cờ
            if (Edited == null)
                _service.Add(p);
            else
            {
                //p.ProductID = int.Parse(txtProductID.Text);
                p.ProductID = Edited.ProductID;

                if (txtName.Text.Trim() == Edited.Name && txtPrice.Text.Trim() == Edited.Price.ToString() && 
                    txtDescription.Text.Trim() == Edited.Description.ToString() && int.TryParse(CbCategoryName.SelectedValue?.ToString(), out int CatId) && CatId == Edited.CategoryID)
                {
                    Close();
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Do you really want to update?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;

                _service.Update(p);
            }


            //TODO bat exception

            this.Close();
        }

        private void FillElements(Product x)
        {
            if (x != null)
            {
                txtName.Text = x.Name;
                txtDescription.Text = x.Description;
                txtPrice.Text = x.Price.ToString();
                CbCategoryName.SelectedValue = x.CategoryID;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if ((Edited == null && txtName.Text.IsNullOrEmpty() && txtPrice.Text.IsNullOrEmpty() && 
                txtDescription.Text.IsNullOrEmpty() && CbCategoryName.SelectedValue == null) ||
                (Edited != null && txtName.Text.Trim() == Edited.Name && 
                txtPrice.Text.Trim() == Edited.Price.ToString() && txtDescription.Text.Trim() == Edited.Description.ToString() &&
                int.TryParse(CbCategoryName.SelectedValue?.ToString(), out int CatId) && CatId == Edited.CategoryID))
            {
                Close();
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you really want to cancel?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            Close();
        }


    }
}
