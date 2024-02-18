using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryDetail : ContentPage
    {
        Hisorder send = new Hisorder();
        Hisorder load = new Hisorder();
        int total;
        public ObservableCollection<ProductNonti> ProductObj;
        public HistoryDetail(Hisorder selectproduct)
        {
            InitializeComponent();
            BindingContext = selectproduct;
            send = selectproduct;

            NavigationPage.SetHasNavigationBar(this, false);

            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                totallabel.Text = "Total";
                thblabel.Text = "THB";
            }
            else
            {
                totallabel.Text = "รวม";
                thblabel.Text = "บาท";
            }

            if (send.status == "Confirm receipt")
			{
                if (Application.Current.Properties["language"].ToString() == "eng.png")
                {
                    BTN.Text = "Confirm";
                }
                else
                {
                    BTN.Text = "ยืนยัน";
                }
                BTN.BackgroundColor = Color.FromHex("3A4857");
                LoadS2();
            }
            else if (send.status == "Shipping")
            {
                if (Application.Current.Properties["language"].ToString() == "eng.png")
                {
                    BTN.Text = "Shipping";
                }
                else
                {
                    BTN.Text = "กำลังจัดส่ง";
                }
                LoadS1();
            }
            else if (send.status == "Completed")
            {
                if (Application.Current.Properties["language"].ToString() == "eng.png")
                {
                    BTN.Text = "Completed";
                }
                else
                {
                    BTN.Text = "เสร็จสิ้น";
                }
                LoadS3();
            }
        }

        
        public async void LoadS2()
        {
            var uri = new Uri("https://vstorex.com/testmobile/hislist.php?mem_id=" + Application.Current.Properties["user_id"] + "&orderkey=" + send.orderkey);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<ProductNonti>>(content);
                ProductObj = new ObservableCollection<ProductNonti>(Items);
                Products.ItemsSource = ProductObj;
            }
            for (int i = 0; i < ProductObj.Count; i++)
            {
                total += (int.Parse(ProductObj[i].price) * int.Parse(ProductObj[i].number));
            }
            Total.Text = string.Format("{0:#,0}", Convert.ToDecimal(total));
        }
        
        public async void LoadS1()
        {
            var uri = new Uri("https://vstorex.com/testmobile/hislist1.php?mem_id=" + Application.Current.Properties["user_id"] + "&orderkey=" + send.orderkey);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<ProductNonti>>(content);
                ProductObj = new ObservableCollection<ProductNonti>(Items);
                Products.ItemsSource = ProductObj;
            }
            for (int i = 0; i < ProductObj.Count; i++)
            {
                total += (int.Parse(ProductObj[i].price) * int.Parse(ProductObj[i].number));
            }
            Total.Text = string.Format("{0:#,0}", Convert.ToDecimal(total));
        }
        public async void LoadS3()
        {
            var uri = new Uri("https://vstorex.com/testmobile/hislist3.php?mem_id=" + Application.Current.Properties["user_id"] + "&orderkey=" + send.orderkey);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<ProductNonti>>(content);
                ProductObj = new ObservableCollection<ProductNonti>(Items);
                Products.ItemsSource = ProductObj;
            }
            for (int i = 0; i < ProductObj.Count; i++)
            {
                total += (int.Parse(ProductObj[i].price) * int.Parse(ProductObj[i].number));
            }
            Total.Text = string.Format("{0:#,0}", Convert.ToDecimal(total));
        }
        void BackPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new History());
        }
        async void Confirm(object sender, EventArgs e)
        {
            if(send.status == "Confirm receipt")
			{
                HttpClient myClient = new HttpClient();
                var uri = new Uri("https://vstorex.com/testmobile/hisset.php?orderkey=" + send.orderkey + "&mem_id=" + Application.Current.Properties["user_id"]);
                await myClient.GetAsync(uri);
                await PopupNavigation.Instance.PushAsync(new ReviewPop(send.orderkey));
                send.status = "Completed";
                App.Current.MainPage = new NavigationPage(new HistoryDetail(send));
            }
        }
    }
}