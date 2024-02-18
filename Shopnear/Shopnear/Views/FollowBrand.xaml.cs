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
    public partial class FollowBrand : ContentPage
    {
        public ObservableCollection<follow> ProductObj;
        public FollowBrand()
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
                followlabel.Text = "Follow Brand";
            }
            else
            {
                followlabel.Text = "ร้านที่คุณสนใจ";
            }
        }
        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/showband.php?mem_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<follow>>(content);
                ProductObj = new ObservableCollection<follow>(Items);
                Products.ItemsSource = ProductObj;
            }
        }
        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}