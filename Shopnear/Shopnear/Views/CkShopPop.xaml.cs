using Newtonsoft.Json;
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
    public partial class CkShopPop : ContentPage
    {
        OrderShop send = new OrderShop();
        public ObservableCollection<OrderShop> ProductObj;
        public CkShopPop(OrderShop selectproduct)
        {
            InitializeComponent();
            BindingContext = selectproduct;
            send = selectproduct;
            NavigationPage.SetHasNavigationBar(this, false);
            Load();
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                orderidlabel.Text = "Order ID";
                totalpaylabel.Text = "Total Payment";
                accountnumlabel.Text = "Account Number";
                confirmbut.Text = "Confirm";
            }
            else
            {
                orderidlabel.Text = "หมายเลขรายการ";
                totalpaylabel.Text = "ชำระเงินทั้งหมด";
                accountnumlabel.Text = "หมายเลขบัญชี";
                confirmbut.Text = "ยืนยัน";
            }
        }
        public async void Load()
        {
            var uri = new Uri("https://vstorex.com/testmobile/ckshoppop.php?shop_id=" + send.shop_id + "&orderkey=" + send.orderkey);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<OrderShop>>(content);
                ProductObj = new ObservableCollection<OrderShop>(Items);
            }

            Total.Text = string.Format("{0:#,0}", Convert.ToDecimal(ProductObj[0].pricetotal));
            AccountNumber.Text = ProductObj[0].pay_number;
            Bank.Text = ProductObj[0].pay_bank;
            PayImg.Source = ProductObj[0].pay_img;
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            Application.Current.MainPage = new AdminPage();
        }
        async void Confirm(object sender, EventArgs e)
        {
            HttpClient myClient = new HttpClient();
            var uri = new Uri("https://vstorex.com/testmobile/ckshopset.php?orderkey=" + send.orderkey + "&shop_id=" + send.shop_id);
            await myClient.GetAsync(uri);
            await Navigation.PopModalAsync();
        }
    }
}