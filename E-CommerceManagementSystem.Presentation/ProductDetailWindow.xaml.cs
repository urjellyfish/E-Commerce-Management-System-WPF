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
    /// Interaction logic for ProductDetailWindow.xaml
    /// </summary>
    public partial class ProductDetailWindow : Window
    {
        public Product Edited {  get; set; }
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
                txtProductID.Text = (_service.GetMaxId() + 1).ToString();
                txtProductID.IsEnabled = false;
            }
            else
            {
                WindowMode.Text = "Update a Product";
                txtProductID.Text = Edited.ProductID.ToString();
                txtProductID.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Product p = new();

            // Validate inputs
            //if (string.IsNullOrWhiteSpace(txtProjectId.Text) ||
            //    string.IsNullOrWhiteSpace(txtProjectTitle.Text) ||
            //    string.IsNullOrWhiteSpace(txtResearchField.Text) ||
            //    string.IsNullOrWhiteSpace(dpStartDate.Text) ||
            //    string.IsNullOrWhiteSpace(dpEndDate.Text) ||
            //    string.IsNullOrWhiteSpace(txtBudget.Text) ||
            //        cbLeadResearcher.SelectedValue == null)
            //{
            //    MessageBox.Show("Please fill in all fields.");
            //    return;
            //}

            //if (dpStartDate.SelectedDate.Value > dpEndDate.SelectedDate.Value)
            //{
            //    MessageBox.Show("Start date must be before end date.");
            //    return;
            //}


            //string title = txtProjectTitle.Text.Trim();

            //if (title.Length > 100 || title.Length < 5)
            //{
            //    MessageBox.Show("Project title must be between 5 and 100 characters.");
            //    return;
            //}

            //if (title.Any(c => "specialCharHere".Contains(c)))
            //{
            //    MessageBox.Show("Project title cannot contain special characters such as ...");
            //    return;
            //}

            //string[] words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //foreach (var word in words)
            //{
            //    if (!(char.IsUpper(word[0]) || (char.IsDigit(word[0]) && word[0] != '0')))
            //    {
            //        MessageBox.Show("Each word in the project title must start with an uppercase letter or be a digit (0-9).");
            //        return;
            //    }
            //}

            //if (!decimal.TryParse(txtBudget.Text, out decimal budget) || budget < 0)
            //{
            //    MessageBox.Show("Budget must be a valid positive number.");
            //    return;
            //}

            p.Name = txtName.Text;
            p.Description = txtDescription.Text;
            p.Price = decimal.Parse(txtPrice.Text);
            if(CbCategoryName.SelectedValue != null)
                p.CategoryID = (int)CbCategoryName.SelectedValue;

            //ktra cờ
            if (Edited == null)
                _service.Add(p);
            else
            {
                p.ProductID = int.Parse(txtProductID.Text);
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
            this.Close();
        }
    }
}
