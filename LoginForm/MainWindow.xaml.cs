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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:3000/api/");
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

        }

        private async void loginButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter a username!", "Error");
                return;
            }

            if (passwordBox.Password == String.Empty)
            {
                MessageBox.Show("Please enter a password!", "Error");
                return;
            }
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            JObject loginData = new JObject(new JProperty("username", usernameBox.Text), new JProperty("password", passwordBox.Password));

            var response = await client.PostAsync("authenticate", new StringContent(loginData.ToString(), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                HttpHeaders headers = response.Headers;
                IEnumerable<string> values;
                string token = String.Empty;
                if (headers.TryGetValues("token", out values))
                {
                    token = values.First();
                }
                this.Hide();
                Checkout checkout = new Checkout(token);
                checkout.Owner = Application.Current.MainWindow;
                checkout.Show();
            }
            else
            {
                MessageBox.Show("Invalid Credentials", "Error");
            }
        }
    }
}
