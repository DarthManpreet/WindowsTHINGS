using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Requests.xaml
    /// </summary>
    public partial class Requests : Window
    {
        HttpClient client = new HttpClient();
        string token;
        public Requests(string token)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            dateNeeded.DisplayDateStart = DateTime.Today;
            dateNeeded.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today));
            itemDescription.TextWrapping = TextWrapping.Wrap;
            itemDescription.AcceptsReturn = true;
            anyInfoBox.TextWrapping = TextWrapping.Wrap;
            anyInfoBox.AcceptsReturn = true;

            client.BaseAddress = new Uri("https://localhost:3000/api/");
            client.DefaultRequestHeaders.Add("x-access-token", token);
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.token = token;
        }

        private void checkoutMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Checkout checkout = new Checkout(token);
            checkout.Owner = Application.Current.MainWindow;
            checkout.Show();
        }

        private void checkinMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Checkin checkin = new Checkin(token);
            checkin.Owner = Application.Current.MainWindow;
            checkin.Show();
        }

        private async void sendRequest_ClickAsync(object sender, RoutedEventArgs e)
        {
            JObject requestData = new JObject(new JProperty("itemName", itemName.Text), new JProperty("quantityNeeded", quantityNeeded.Text),
                                              new JProperty("description", itemDescription.Text), new JProperty("message", anyInfoBox.Text),
                                              new JProperty("personName", personName.Text), new JProperty("email", emailBox.Text),
                                              new JProperty("date", dateNeeded.Text));

            var response = await client.PostAsync("a/request/", new StringContent(requestData.ToString(), Encoding.UTF8, "application/json"));
            if(!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Unable to process request! Please try again", "Error");
            }
            else
            {
                MessageBox.Show("Request submitted!", "Success");
            }

        }
    }
}
