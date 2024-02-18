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
using Rg.Plugins.Popup.Services;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPop 
    {
        public ObservableCollection<Product> ProductObj;

        public SearchPop(string searchValue)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ReadDataAsync(searchValue);
            search.Text = searchValue;
            LanguageSwif();

        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                search.Placeholder = "Search";
            }
            else
            {
                search.Placeholder = "ค้นหา";
            }
        }
        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectproduct = (e.CurrentSelection.FirstOrDefault() as Product);
            //Products.SelectedItems.Clear(); 
            //DisplayAlert("test", selectproduct.id, "OK");
            await Navigation.PushAsync(new ProductDetail(selectproduct));
            await PopupNavigation.Instance.PopAsync();
        }

        public async void ReadDataAsync(string searchValue)
        {
            var uri = new Uri("https://vstorex.com/testmobile/search.php?search=" + searchValue);
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

        private void OnSearch(object sender, EventArgs e)
        {
            ReadDataAsync(search.Text);
        }


    }
}