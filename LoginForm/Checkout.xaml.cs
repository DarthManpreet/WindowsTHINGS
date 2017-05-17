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
        string token = String.Empty;

        public Checkout(string token)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.token = token;
            client.BaseAddress = new Uri("https://localhost:3000/api/");
            client.DefaultRequestHeaders.Add("x-access-token", token);
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

        private void addCart_Click(object sender, RoutedEventArgs e)
        {   
            if (cart == null)
            {
                cart = inventory.Clone();
                cart.Columns.Add("desiredAmount");
                cart.Columns["desiredAmount"].DefaultValue = 1;
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

        private async void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = new List<string>();
            for(int i = 0; i < cart.Rows.Count; i++)
            {
                var response = await client.PostAsync("a/checkout/" + cart.Rows[i]["item_id"] + "/" + "admin" + "/" + cart.Rows[i]["desiredAmount"], null);
                if (!response.IsSuccessStatusCode)
                {
                    errors.Add(cart.Rows[i]["name"].ToString());
                }
            }

            //Delete all rows from the cart datatable
            cart.Clear();

            //Refresh inventory
            await loadInventory();

            if(errors.Count > 0)
            {
                MessageBox.Show("Unable to checkout the following item(s): \n" + string.Join(Environment.NewLine, errors));
            }
            else
            {
                MessageBox.Show("All items successfully checked out!");
            }
        }

        //Menu Bar Headers
        private void checkinHeader_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Checkin checkin = new Checkin(token);
            checkin.Owner = Application.Current.MainWindow;
            checkin.Show();
        }

        private void requestHeader_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Requests request = new Requests(token);
            request.Owner = Application.Current.MainWindow;
            request.Show();
        }

        private void historyRecent_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            HistoryRecent recent = new HistoryRecent(token);
            recent.Owner = Application.Current.MainWindow;
            recent.Show();
        }

        private void historyItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            HistoryItem item = new HistoryItem(token);
            item.Owner = Application.Current.MainWindow;
            item.Show();
        }

        private void historyDate_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            HistoryRange date = new HistoryRange(token);
            date.Owner = Application.Current.MainWindow;
            date.Show();
        }

        private void shoppingList_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Shopping_List list = new Shopping_List(token);
            list.Owner = Application.Current.MainWindow;
            list.Show();
        }

        private void stats_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Statistics stats = new Statistics(token);
            stats.Owner = Application.Current.MainWindow;
            stats.Show();
        }
    }
}
