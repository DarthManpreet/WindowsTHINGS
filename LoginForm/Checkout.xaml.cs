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

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        HttpClient client = new HttpClient();
        public Checkout()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:3000/api/");
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            loadInventory();
        }

        private async void loadInventory()
        {
            string response = await client.GetStringAsync("view");
            //MessageBox.Show(response);
            var dt = JsonConvert.DeserializeObject<DataTable>(response);
            MessageBox.Show(dt.Rows.ToString());
            checkoutGrid.ItemsSource = dt.AsDataView();
        }
    }
}
