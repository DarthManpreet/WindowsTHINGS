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
    /// Interaction logic for Shopping_List.xaml
    /// </summary>
    public partial class Shopping_List : Window
    {
        HttpClient client = new HttpClient();
        string token;
        DataTable list = new DataTable();
        public Shopping_List(string token)
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
            Task t = loadList();
        }

        private async Task loadList()
        {
            string response = await client.GetStringAsync("a/admin/shopping_list");
            list = JsonConvert.DeserializeObject<DataTable>(response);
            shoppingList.ItemsSource = list.AsDataView();
        }


        //Menu Bars
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

        private void historyRecent_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            HistoryRecent recent = new HistoryRecent(token);
            recent.Owner = Application.Current.MainWindow;
            recent.Show();
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
