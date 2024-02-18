using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using Shopnear.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CkShop 
    {
        public ObservableCollection<OrderShop> Item;

        public CkShop()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Load();
           // LanguageSwif();
        }

        /*void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                orderlabel.Text = "Order";
            }
            else
            {
                orderlabel.Text = "รายการ";
            }
        }*/

        public async void Load()
        {
            var uri = new Uri("https://vstorex.com/testmobile/shoporder.php");
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<OrderShop>>(content);
                Item = new ObservableCollection<OrderShop>(Items);
                Carts.ItemsSource = Item;
            }
        }

        async void OnCollectionViewSelectionChanged(object sender, ItemTappedEventArgs e)
        {
            var selectproduct = e.Item as OrderShop;
            await Navigation.PushModalAsync(new CkShopPop(selectproduct));
        }
        void OnSignoutClick(object sender, EventArgs e)
        {
            string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "session.json");

            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }

            Application.Current.Properties["user_id"] = null;
            Application.Current.Properties["user_Email"] = null;
            Application.Current.Properties["user_name"] = null;
            Application.Current.Properties["user_picture"] = null;
            Application.Current.Properties["user_phone"] = null;
            Application.Current.Properties["user_location"] = null;
            Application.Current.Properties["user_account_number"] = null;
            Application.Current.Properties["user_status"] = null;

            Application.Current.Properties["shop_id"] = null;
            Application.Current.Properties["shop_name"] = null;
            Application.Current.Properties["shop_picture"] = null;
            Application.Current.Properties["shop_agent"] = null;
            Application.Current.Properties["shop_qr"] = null;
            Application.Current.Properties["shop_bank"] = null;
            Application.Current.Properties["shop_account_no"] = null;

            App.Current.MainPage = new loginPage();
        }

    }
}