using Shopnear.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopPage : ContentPage
    {
        public ObservableCollection<Product> ProductObj;
        public ObservableCollection<OrderUser> Nonti;
        public ShopPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            profile.Source = (string)Application.Current.Properties["shop_picture"];
            nameShop.Text = (string)Application.Current.Properties["shop_name"];
            LoadProduct();
            LoadNonti();
        }
        private async void LoadNonti()
        {
            var uri = new Uri("https://vstorex.com/testmobile/ordershop.php?shop_id=" + Application.Current.Properties["shop_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<OrderUser>>(content);
                Nonti = new ObservableCollection<OrderUser>(Items);
                NontiNum.Text = Nonti.Count.ToString();
            }
        }
        private async void LoadProduct()
		{
            var uri = new Uri("https://vstorex.com/testmobile/shoppro.php?shop_id=" + Application.Current.Properties["shop_id"].ToString());
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<Product>>(content);
                ProductObj = new ObservableCollection<Product>(Items);
                Products.ItemsSource = ProductObj;
            }
        }
        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectproduct = (e.CurrentSelection.FirstOrDefault() as Product);
            //Products.SelectedItems.Clear(); 
            //DisplayAlert("test", selectproduct.id, "OK");
            await Navigation.PushAsync(new ShopProductDetail(selectproduct));

            //selectproduct.thname, selectproduct.img, selectproduct.story));
        }

        void back(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());

        }
        void AddProduct(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddProduct());
        }

        void Notification(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Notification());
        }



    }
}