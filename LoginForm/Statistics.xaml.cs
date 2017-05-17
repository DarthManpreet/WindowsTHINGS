using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        HttpClient client = new HttpClient();
        string token;
        DataTable stats = new DataTable();

        public Statistics(string token)
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
            Task t = loadStats();
        }

        public async Task loadStats()
        {
            //Load the inventory
            string response = await client.GetStringAsync("view");
            stats = JsonConvert.DeserializeObject<DataTable>(response);
           
            stats.Columns.Add("Weekly Average");
            stats.Columns.Add("Net per Day");

            var tasks = new List<Task<string>>();
            for(int i = 0; i < stats.Rows.Count; i++)
            {
                //Weekly Average
                string resp = await client.GetStringAsync("a/admin/stats/weeklyavg/" + stats.Rows[i]["item_id"]);
                if (resp != "[]")
                {
                    JArray avg = JArray.Parse(resp);
                    JObject avgs= JObject.Parse(avg.First.ToString());             
                    stats.Rows[i]["Weekly Average"] = ((float)avgs["weekly_avg"]).ToString("0.00");
                }
                else
                {
                    stats.Rows[i]["Weekly Average"] = 0.00;
                }

                //Net per day
                resp = await client.GetStringAsync("a/admin/stats/netperday/" + stats.Rows[i]["item_id"]);
                if (resp != "[]")
                {
                    JArray avg = JArray.Parse(resp);
                    JObject avgs = JObject.Parse(avg.First.ToString());
                    stats.Rows[i]["Net per Day"] = ((float)avgs["net_change"]).ToString("0.00");
                }
                else
                {
                    stats.Rows[i]["Net per Day"] = 0.00;
                }
            }
            statsGrid.ItemsSource = stats.AsDataView();
        }


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

        private void shoppingListHeader_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Shopping_List list = new Shopping_List(token);
            list.Owner = Application.Current.MainWindow;
            list.Show();
        }
    }
}
