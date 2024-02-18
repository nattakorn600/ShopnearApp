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
using System.Windows.Input;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        public ObservableCollection<Product> ProductObj;
        public ObservableCollection<ImgProduct> ImgPros;

        public ProductPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            user_picture.Source = (string)Application.Current.Properties["user_picture"];
            ReadDataAsync();
            Loadpromotion();

            Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
            {
                Promotion.Position = (Promotion.Position + 1) % ImgPros.Count;
                return true;
            }));
        }
        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/product.php");
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
        void ToProfilePage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());
        }
        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectproduct = (e.CurrentSelection.FirstOrDefault() as Product);
            //Products.SelectedItems.Clear(); 
            //DisplayAlert("test", selectproduct.id, "OK");
            await Navigation.PushAsync(new ProductDetail(selectproduct));

            //selectproduct.thname, selectproduct.img, selectproduct.story));
        }

        public async void Loadpromotion()
        {
            var uri = new Uri("https://vstorex.com/testmobile/promotion.php");
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<ImgProduct>>(content);
                ImgPros = new ObservableCollection<ImgProduct>(Items);
                Promotion.ItemsSource = ImgPros;
            }
        }
        private void Pinmap(object sender, EventArgs e)
		{
            Navigation.PushAsync(new MapPage());
		}
    }
}
