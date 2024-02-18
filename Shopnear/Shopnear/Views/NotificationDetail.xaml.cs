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
    public partial class NotificationDetail : ContentPage
    {
        OrderNonti send = new OrderNonti();
        int total;
        public ObservableCollection<ProductNonti> ProductObj;
        public NotificationDetail(OrderNonti selectproduct)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = selectproduct;
            send = selectproduct;
            ReadDataAsync();
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                totallabel.Text = "Total";
                thblabel.Text = "THB";
                addresslabel.Text = "Address";
                confirmlabel.Text = "Confirm";
            }
            else
            {
                totallabel.Text = "รวม";
                thblabel.Text = "บาท";
                addresslabel.Text = "ที่อยู่";
                confirmlabel.Text = "ยืนยัน";
            }
        }

        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/nonti.php?shop_id=" + Application.Current.Properties["shop_id"] + "&orderkey=" + send.orderkey);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<ProductNonti>>(content);
                ProductObj = new ObservableCollection<ProductNonti>(Items);
                Products.ItemsSource = ProductObj;
            }
            for(int i=0; i<ProductObj.Count; i++)
			{
                total += (int.Parse(ProductObj[i].price) * int.Parse(ProductObj[i].number));
			}
            Total.Text = string.Format("{0:#,0}", Convert.ToDecimal(total));
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Notification());
        }
        async void Confirm(object sender, EventArgs e)
        {
            HttpClient myClient = new HttpClient();
            var uri = new Uri("https://vstorex.com/testmobile/nontiset.php?orderkey=" + send.orderkey + "&shop_id=" + Application.Current.Properties["shop_id"]);
            await myClient.GetAsync(uri);
            await Navigation.PushAsync(new Notification());
        }
    }
}