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
        private int searchID = -1;
        private string searchName = "";

        public MainWindow(IConnectionFactory connectionFactory)
        {
            unitOfWork = new UnitOfWork(connectionFactory);
            products = unitOfWork.Products;
            InitializeComponent();
            All_Items.ItemsSource = products.GetAll();
        }
        private void Search()
        {
            List<Product> results = new List<Product>();

            try
            {
                if(searchID == -1)
                {
                    results.AddRange(products.GetByName(searchName));
                }
                else
                {
                    results.Add(products.GetById(searchID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SearchResults.ItemsSource = results;
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

                // Add the product in the database
                products.Add(newProd);

                // Refresh all items list
                All_Items.ItemsSource = products.GetAll();

                // Refresh the search results if they are affected by the new addition
                if (newProd.Name == searchName || newProd.Id == searchID)
                {
                    Search();
                }
                NewItemName.Text = "";
                NewItemPrice.Text = "";
                NewItemStock.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (RemoveAmount.Text != "")
                {
                    try
                    {
                        product.Quantity -= int.Parse(RemoveAmount.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (NewPrice.Text != "")
                {
                    try
                    {
                        product.Price = decimal.Parse(NewPrice.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (NewName.Text != "")
                {
                    product.Name = NewName.Text;
                }
                // Change the product in the database
                products.Update(product);

                // Refresh all items list
                All_Items.ItemsSource = products.GetAll();

                // Refresh search results if affected
                if(product.Name == searchName || id == searchID)
                {
                    Search();
                }

                NewName.Text = "";
                NewPrice.Text = "";
                AddAmount.Text = "";
                RemoveAmount.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                    throw new ArgumentException("Product was not found");
                }

                // Remove product from database
                products.Delete(product);

                // Refresh all items list
                All_Items.ItemsSource = products.GetAll();

                // Refresh search results if the deleted item was in the search results
                if (product.Name == searchName || id == searchID)
                {
                    Search();
                }

                ID.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            try
            {
                if(IDSearchInput.Text == "")
                {
                    searchID = -1;
                }
                else
                {
                    searchID = int.Parse(IDSearchInput.Text);
                }
            }
            catch
            {
                MessageBox.Show("Invalid Id");
            }
            searchName = NameSearchInput.Text;

            Search();

            NameSearchInput.Text = "";
            IDSearchInput.Text = "";
        }

        private void OnSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            Product selectedItem = (Product)((ListView)sender).SelectedItem;

            // clear all editing textboxes
            ID.Text = "";
            NewName.Text = "";
            NewPrice.Text = "";

            // check if the selected item actually exists (it may have been deleted and threw this event)
            if (selectedItem != null)
            {
                // set editing textboxes
                ID.Text = selectedItem.Id.ToString();
                NewName.Text = selectedItem.Name.ToString();
                NewPrice.Text = selectedItem.Price.ToString();
            }
        }
    }
}
