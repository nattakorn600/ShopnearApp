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
    public partial class History : ContentPage
    {
        public ObservableCollection<Hisorder> Item1;
        public ObservableCollection<Hisorder> Item2;
        public ObservableCollection<Hisorder> Item3;
        public ObservableCollection<Hisorder> ListItem = new ObservableCollection<Hisorder>();
        public History()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadorderS2();
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                historylabel.Text = "History";
            }
            else
            {
                historylabel.Text = "ประวัติการซื้อ";
            }
        }

        public void Load(ObservableCollection<Hisorder> item)
		{
            if(item.Count>0)
			{
                for (int i = 0; i < item.Count; i++)
                {
                    ListItem.Add(item[i]);
                }
            }

            Carts.ItemsSource = ListItem;
        }
        public async void LoadorderS2()
        {
            var uri = new Uri("https://vstorex.com/testmobile/hisorder.php?mem_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<Hisorder>>(content);
                Item2 = new ObservableCollection<Hisorder>(Items);
                Load(Item2);
                LoadorderS1();
            }
        }
        public async void LoadorderS1()
        {
            var uri = new Uri("https://vstorex.com/testmobile/hisorder1.php?mem_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<Hisorder>>(content);
                Item1 = new ObservableCollection<Hisorder>(Items);
                Load(Item1);
                LoadorderS3();
            }
        }
       
        public async void LoadorderS3()
        {
            var uri = new Uri("https://vstorex.com/testmobile/hisorder3.php?mem_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<Hisorder>>(content);
                Item3 = new ObservableCollection<Hisorder>(Items);
                Load(Item3);
            }
        }

        async void OnCollectionViewSelectionChanged(object sender, ItemTappedEventArgs e)
        {
            var selectproduct = e.Item as Hisorder;
            await Navigation.PushAsync(new HistoryDetail(selectproduct));
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());
        }
    }
}