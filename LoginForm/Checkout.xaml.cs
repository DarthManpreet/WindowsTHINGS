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
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        HttpClient client = new HttpClient();
        DataTable inventory;
        DataTable cart = null;

        public Checkout()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:3000/api/");
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Task t = loadInventory();
        }

        private async Task loadInventory()
        {
            string response = await client.GetStringAsync("view");
            inventory = JsonConvert.DeserializeObject<DataTable>(response);
            checkoutGrid.ItemsSource = inventory.AsDataView();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
            if (!e.Handled)
            {
                int rowIndex = checkoutCart.Items.IndexOf(checkoutCart.CurrentItem);
                e.Handled = Int32.Parse(e.Text) > Int32.Parse(cart.Rows[rowIndex]["quantity"].ToString());
            }
        }

        private void addCart_Click(object sender, RoutedEventArgs e)
        {   
            if (cart == null)
            {
                cart = inventory.Clone();
            }

            var selected = checkoutGrid.SelectedItems;
            foreach (DataRowView item in selected)
            {
                cart.ImportRow(item.Row);
            }

            checkoutCart.ItemsSource = cart.AsDataView();
        }

        private void removeCart_Click(object sender, RoutedEventArgs e)
        {
            if(cart != null)
            {
                while (checkoutCart.SelectedItems.Count > 0)
                {
                    cart.Rows.RemoveAt(checkoutCart.SelectedIndex);
                }
            }
        }

        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
