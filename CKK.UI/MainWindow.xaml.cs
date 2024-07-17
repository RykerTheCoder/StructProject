using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
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
using System.Xaml;
using CKK.DB.Interfaces;
using CKK.DB.UOW;
using CKK.Logic.Interfaces;
using CKK.Logic.Models;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IProductRepository products;

        public MainWindow(IConnectionFactory connectionFactory)
        {
            unitOfWork = new UnitOfWork(connectionFactory);
            products = unitOfWork.Products;
            InitializeComponent();
            All_Items.ItemsSource = products.GetAll();
        }
        private void OnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product newProd = new Product();
                if (NewItemName.Text == "")
                {
                    throw new ArgumentException();
                }
                newProd.Name = NewItemName.Text;
                newProd.Price = decimal.Parse(NewItemPrice.Text);
                newProd.Quantity = int.Parse(NewItemStock.Text);

                products.Add(newProd);
                All_Items.Items.Add(products);

                NewItemName.Text = "";
                NewItemPrice.Text = "";
                NewItemStock.Text = "";
            }
            catch
            {
                MessageBox.Show("Error: Cannot Create Item. Maybe you used an invalid input?");
            }
        }

        private void OnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int id = int.Parse(ID.Text);
                Product product = products.GetById(id);

                if (product == null)
                {
                    throw new ArgumentException();
                }

                if (AddAmount.Text != "")
                {
                    try
                    {
                        product.Quantity += int.Parse(AddAmount.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid add amount");
                    }
                }
                if (RemoveAmount.Text != "")
                {
                    try
                    {
                        product.Quantity -= int.Parse(RemoveAmount.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid remove amount");
                    }
                }
                if (NewPrice.Text != "")
                {
                    try
                    {
                        product.Price = decimal.Parse(NewPrice.Text);
                    }
                    catch
                    {

                        MessageBox.Show("Invalid Price");
                    }
                }
                if (NewName.Text != "")
                {
                    product.Name = NewName.Text;
                }

                products.Update(product);
                All_Items.ItemsSource = products.GetAll();

                NewName.Text = "";
                NewPrice.Text = "";
                AddAmount.Text = "";
                RemoveAmount.Text = "";

            }
            catch
            {
                MessageBox.Show("Invalid ID input or ID was not found");
            }
        }
        private void OnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(ID.Text);
                Product product = products.GetById(id);

                if (product == null)
                {
                    throw new ArgumentException();
                }

                products.Delete(product);

                All_Items.ItemsSource = products.GetAll();
            }
            catch
            {
                MessageBox.Show("Invalid ID input or ID was not found");
            }
        }

        private void OnSortQuantity_Click(object sender, RoutedEventArgs e)
        {
            var sortedProducts = products.GetAll().OrderBy(x => x.Quantity);
            All_Items.ItemsSource = sortedProducts;
        }

        private void OnSortPrice_Click(object sender, RoutedEventArgs e)
        {
            var sortedProducts = products.GetAll().OrderBy(x => x.Price);
            All_Items.ItemsSource = sortedProducts;
        }

        private void OnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Product> results = new List<Product>();

            if (IDSearchInput.Text != string.Empty)
            {
                try
                {
                    results.Add(products.GetById(int.Parse(IDSearchInput.Text)));
                }
                catch
                {
                    MessageBox.Show("Invalid ID input or ID wasn't found");
                }
            }
            else if (NameSearchInput.Text != string.Empty)
            {
                results = products.GetByName(NameSearchInput.Text);
            }
            else
            {
                MessageBox.Show("No input.");
            }

            SearchResults.ItemsSource = results;
        }

        private void OnSearchSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            // clear all editing textboxes
            ID.Text = "";
            NewName.Text = "";
            NewPrice.Text = "";

            Product selectedItem = (Product)SearchResults.SelectedItem;

            ID.Text = selectedItem.Id.ToString();
            NewName.Text = selectedItem.Name.ToString();
            NewPrice.Text = selectedItem.Price.ToString();

        }
    }
}
