using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CKK.DB.Interfaces;
using CKK.DB.UOW;
using CKK.Logic.Exceptions;
using CKK.Logic.Models;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UnitOfWork unitOfWork; // variable to provide access to the repositories connected to the database

        private readonly IProductRepository products; // this variable is just here so that I dont have to write unitOfWork.Products a ton
        private int searchID = -1;
        private string searchName = "";

        public MainWindow(IConnectionFactory connectionFactory)
        {
            //initialize instance variables
            unitOfWork = new UnitOfWork(connectionFactory);
            products = unitOfWork.Products;
            InitializeComponent();

            // Display the products on the "All Items" list
            All_Items.ItemsSource = products.GetAll();
        }
        // Searches the database using unitOfWork
        private void Search()
        {
            List<Product> results = new List<Product>();

            try
            {
                if (searchID == -1) // if nothing was entered into the id box then search for the name
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Display the resulting data
            SearchResults.ItemsSource = results;
        }
        // When the "Add Item" button is clicked
        private void OnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product newProd = new Product();

                string newName = NewItemName.Text;
                decimal newPrice;
                int newQuantity;

                // validate price
                try
                {
                    newPrice = decimal.Parse(NewItemPrice.Text);
                }
                catch
                {
                    throw new ArgumentException("Price is invalid");
                }
                if (newPrice >= 0)
                {
                    newProd.Price = newPrice;
                }
                else
                {
                    throw new ArgumentException("Price cannot be negative");
                }

                // validate quantity
                try
                {
                    newQuantity = int.Parse(NewItemStock.Text);
                }
                catch
                {
                    throw new ArgumentException("Stock is invalid");
                }
                if (newQuantity >= 0)
                {
                    newProd.Quantity = newQuantity;
                }
                else
                {
                    throw new ArgumentException("Stock cannot be negative");
                }

                if (newName != "") // check for if the name was entered
                {
                    newProd.Name = newName;
                }
                else
                {
                    throw new ArgumentException("Name was not entered");
                }

                // Add the product in the database
                products.Add(newProd);

                // Refresh all items list
                All_Items.ItemsSource = products.GetAll();

                // Refresh the search results if they are affected by the new addition
                if (newProd.Name == searchName || newProd.Id == searchID)
                {
                    Search();
                }

                // reset textboxes
                NewItemName.Text = "";
                NewItemPrice.Text = "";
                NewItemStock.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Event for when "Save Changes" button is clicked
        private void OnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int id;

                try
                {
                    id = int.Parse(ID.Text);
                }
                catch
                {
                    throw new ArgumentException("Id is invalid");
                }

                Product product = products.GetById(id); // retrieve the product from the database

                if (product == null) // checks if the product was found or not
                {
                    throw new ProductDoesNotExistException("Product could not be found");
                }

                // Change the product accordingly (with validation)
                if (AddAmount.Text != "")
                {
                    try
                    {
                        product.Quantity += int.Parse(AddAmount.Text);
                    }
                    catch
                    {
                        throw new ArgumentException("Add amount is invalid");
                    }
                }
                if (RemoveAmount.Text != "")
                {
                    int removeAmount;

                    try
                    {
                        removeAmount = int.Parse(RemoveAmount.Text);
                    }
                    catch
                    {
                        throw new ArgumentException("Remove amount is invalid");
                    }
                    

                    if (product.Quantity >= removeAmount) // prevent quantity from going negative
                    {
                        product.Quantity -= removeAmount;
                    }
                    else
                    {
                        throw new InventoryItemStockTooLowException("Cannot remove an amount greater than the stock");
                    }
                }
                if (NewPrice.Text != "")
                {
                    decimal newPrice;
                    try
                    {
                        newPrice = decimal.Parse(NewPrice.Text);
                    }
                    catch
                    {
                        throw new ArgumentException("New price is invalid");
                    }

                    if (newPrice >= 0)
                    {
                        product.Price = newPrice;
                    }
                    else
                    {
                        throw new ArgumentException("New price cannot be negative");
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
                if (product.Name == searchName || id == searchID)
                {
                    Search();
                }

                // reset textboxes
                NewName.Text = "";
                NewPrice.Text = "";
                AddAmount.Text = "";
                RemoveAmount.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Event for when the "Delete Item" button is clicked
        private void OnDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;

                try
                {
                    id = int.Parse(ID.Text);
                }
                catch
                {
                    throw new ArgumentException("Id is invalid");
                }

                Product product = products.GetById(id);

                if (product == null) //check if the product was found
                {
                    throw new ProductDoesNotExistException("Product could not be found");
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

                //reset id textbox
                ID.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // events for when you click the column headers for the "All Items" list
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
        private void OnSortID_Click(object sender, RoutedEventArgs e)
        {
            var sortedProducts = products.GetAll().OrderBy(x => x.Id);
            All_Items.ItemsSource = sortedProducts;
        }

        private void OnSortName_Click(object sender, RoutedEventArgs e)
        {
            var sortedProducts = products.GetAll().OrderBy(x => x.Name);
            All_Items.ItemsSource = sortedProducts;
        }
        // Event for when the "Search" button is clicked
        private void OnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    if (IDSearchInput.Text == "") // if id was not entered
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
                    throw new ArgumentException("Id is invalid");
                }

                searchName = NameSearchInput.Text; // search name will not get searched if id was found so it is safe to set it by default

                // Search the database and display the products accordingly
                Search();

                //reset textboxes (not sure if it is more intuitive to do this or not)
                NameSearchInput.Text = "";
                IDSearchInput.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
