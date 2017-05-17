using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for HistoryItem.xaml
    /// </summary>
    public partial class HistoryItem : Window
    {
        HttpClient client = new HttpClient();
        string token;
        DataTable history = new DataTable();

        public HistoryItem(string token)
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
        }

        private async Task loadHistory(int entries, string name)
        {
            string response = await client.GetStringAsync("a/admin/history/by_item/" + name + "/" + entries);
            history = JsonConvert.DeserializeObject<DataTable>(response);
            if (history.Rows.Count == 0)
            {
                MessageBox.Show("No transaction for this item!");
            }
            else
            {
                historyGrid.ItemsSource = history.AsDataView();
            }
        }

        private async void newEntry_ClickAsync(object sender, RoutedEventArgs e)
        {
            int num = -1;
            try
            {
                num = int.Parse(entriesBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid entry format!");
            }

            if (num >= 0)
            {
                if (itemName.Text != string.Empty)
                {
                    await loadHistory(num, itemName.Text);
                }
                else
                {
                    MessageBox.Show("Item Name is Required!");
                }
            }
            else
            {
                MessageBox.Show("Invalid entry number!");
            }
        }

        //Menu Bar Headers
        private void checkoutHeader_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Checkout checkout = new Checkout(token);
            checkout.Owner = Application.Current.MainWindow;
            checkout.Show();
        }

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
