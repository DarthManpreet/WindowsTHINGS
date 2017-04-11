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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if(usernameBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter a username!", "Error");
                return;
            }

            if(passwordBox.Password == String.Empty)
            {
                MessageBox.Show("Please enter a password!", "Error");
                return;
            }

            MessageBox.Show("You entered:\n\tUsername: " + usernameBox.Text + "\n\tPassword: " + passwordBox.Password, "Success");
        }
    }
}
