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
using CKK.Logic.Models;
using CKK.Persistance.Models;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnLogClick(object sender, RoutedEventArgs e)
        {
            Store store = (Store)Application.Current.FindResource("globStore");
            FileStore fileStore = new FileStore();
            MainWindow mainWindow = new MainWindow(fileStore);
            mainWindow.Show();
            Close();

        }
    }
}
