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
    public partial class ShopFormMap : ContentPage
    {
        public ObservableCollection<Product> ProductObj;
        public ObservableCollection<OrderUser> Nonti;
        string shopid;
        public ShopFormMap(string id)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            shopid = id;
            Load();
            LoadProduct();
        }

        private async void Load()
        {
            var uri = new Uri("https://vstorex.com/testmobile/takeshopmap.php?shop_id=" + shopid);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<shop>(content);

                profile.Source = Items.picture;
                nameShop.Text = Items.name;
            }
        }
        private async void LoadProduct()
        {
            var uri = new Uri("https://vstorex.com/testmobile/shoppro.php?shop_id=" + shopid);
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
            await Navigation.PushAsync(new ShopMapDetail(selectproduct));

            //selectproduct.thname, selectproduct.img, selectproduct.story));
        }

        void back(object sender, EventArgs e)
        {
            Navigation.PopAsync();

        }
    }
}