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
using CKK.Logic.Interfaces;
using CKK.Logic.Models;
using CKK.Persistance.Models;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IStore _Store;
        public ObservableCollection<StoreItem> _Items { get; private set; }
        public MainWindow(IStore store)
        {
            _Store = store;
            InitializeComponent();
            _Items = new ObservableCollection<StoreItem>();
            All_Items.ItemsSource = _Items;
            RefreshList();
        }
        private void RefreshList()
        {
            _Items.Clear();
            foreach (StoreItem item in new ObservableCollection<StoreItem>(_Store.GetStoreItems()))
            {
                _Items.Add(item);
            }
        }

        private void OnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newName = NewItemName.Text;
                decimal newPrice = decimal.Parse(NewItemPrice.Text);
                int newQuantity = int.Parse(NewItemStock.Text);
                Product newProd = new Product();

                newProd.Name = newName;
                newProd.Price = newPrice;
                _Store.AddStoreItem(newProd, newQuantity);

                NewItemName.Text = "";
                NewItemPrice.Text = "";
                NewItemStock.Text = "";

                RefreshList();
            }
            catch
            {
                MessageBox.Show("Error: Cannot Create Item. Maybe you used an invalid input?");
            }
        }

        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            string newName = NewName.Text;

            if (ID.Text != "")
            {
                try
                {
                    int id = int.Parse(ID.Text);
                    StoreItem storeItem = _Store.FindStoreItemById(id);
                    if (storeItem == null)
                    {
                        throw new ArgumentException();
                    }

                    if (AddAmount.Text != "")
                    {
                        try
                        {
                            int addAmount = int.Parse(AddAmount.Text);
                            _Store.AddStoreItem(storeItem.Product, addAmount);
                        }
                        catch
                        {
                            MessageBox.Show("Invalid Add Amount");
                        }
                    }
                    if (RemoveAmount.Text != "")
                    {
                        try
                        {
                            int removeAmount = int.Parse(RemoveAmount.Text);
                            _Store.RemoveStoreItem(id, removeAmount);
                        }
                        catch
                        {
                            MessageBox.Show("Invalid Remove Amount");
                        }
                    }
                    if (NewPrice.Text != "")
                    {
                        try
                        {
                            decimal newPrice = decimal.Parse(NewPrice.Text);
                            storeItem.Product.Price = newPrice;
                        }
                        catch
                        {
                            
                            MessageBox.Show("Invalid Price");
                        }
                    }
                    if (newName != "")
                    {
                        storeItem.Product.Name = newName;
                    }
                    NewName.Text = "";
                    NewPrice.Text = "";
                    AddAmount.Text = "";
                    RemoveAmount.Text = "";

                    RefreshList();
                    
                }
                catch
                {
                    MessageBox.Show("Invalid ID Input");
                }
            }
        }

        private void OnRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(ID.Text);
                _Store.DeleteStoreItem(id);

                ID.Text = "";

                RefreshList();
            }
            catch
            {
                MessageBox.Show("Item ID Doesn't Exist");
            }
        }

        private void OnSortQuantity_Click(object sender, RoutedEventArgs e)
        {
            _Items.Clear();
            foreach (StoreItem item in _Store.GetProductsByQuantity())
            {
                _Items.Add(item);
            }
        }

        private void OnSortPrice_Click(object sender, RoutedEventArgs e)
        {
            _Items.Clear();
            foreach (StoreItem item in _Store.GetProductsByPrice())
            {
                _Items.Add(item);
            }
        }

        private void OnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchResults.Items.Clear();
            if(IDSearchInput.Text != string.Empty)
            {
                SearchResults.Items.Add(_Store.FindStoreItemById(int.Parse(IDSearchInput.Text)));
            }
            else if(NameSearchInput.Text != string.Empty)
            {
                foreach(StoreItem item in _Store.GetAllProductsByName(NameSearchInput.Text))
                {
                    SearchResults.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No input.");
            }
        }

        private void OnSearchSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            // clear all editing textboxes
            ID.Text = "";
            NewName.Text = "";
            NewPrice.Text = "";

            StoreItem selectedItem = (StoreItem)SearchResults.SelectedItem;

            ID.Text = selectedItem.Product.Id.ToString();
            NewName.Text = selectedItem.Product.Name.ToString();
            NewPrice.Text= selectedItem.Product.Price.ToString();

        }
    }
}
