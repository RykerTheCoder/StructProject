using System.Windows;
using CKK.DB.UOW;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    // This Page doesn't have any verification currently, so it just directs you to MainWindow when you press the button
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void OnLogClick(object sender, RoutedEventArgs e)
        {
            DatabaseConnectionFactory connectionFactory = new DatabaseConnectionFactory();
            MainWindow mainWindow = new MainWindow(connectionFactory);
            mainWindow.Show();
            Close();

        }
    }
}
