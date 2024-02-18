using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Shopnear.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notification : ContentPage
    {
        public ObservableCollection<OrderNonti> ProductObj;
        public Notification()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ReadDataAsync();
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                orderlabel.Text = "Order";
            }
            else
            {
                orderlabel.Text = "รายการ";
            }
        }
        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/ordershop.php?shop_id=" + Application.Current.Properties["shop_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<OrderNonti>>(content);
                ProductObj = new ObservableCollection<OrderNonti>(Items);
                Products.ItemsSource = ProductObj;
            }
        }

        async void OnCollectionViewSelectionChanged(object sender, ItemTappedEventArgs e)
        {
            var selectproduct = e.Item as OrderNonti;
            await Navigation.PushAsync(new NotificationDetail(selectproduct));
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShopPage());
        }
    }
}