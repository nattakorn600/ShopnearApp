using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CkUserPop : ContentPage
    {
        OrderUser send = new OrderUser();
        public CkUserPop(OrderUser selectproduct)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = selectproduct;
            send = selectproduct;
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                billlabel.Text = "Bill";
                ordernumlabel.Text = "Order Number";
                totalpaylabel.Text = "Total Payment";
                thblabel.Text = "THB";
                payprooflabel.Text = "Payment proof";
                confirmbut.Text = "Confirm";
                calcelbut.Text = "Cancel";
            }
            else
            {
                billlabel.Text = "บิล";
                ordernumlabel.Text = "หมายเลขรายการ";
                totalpaylabel.Text = "ราคาชำระรวม";
                thblabel.Text = "บาท";
                payprooflabel.Text = "หลักฐานการโอน";
                confirmbut.Text = "ยืนยัน";
                calcelbut.Text = "ยกเลิก";
            }
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            Application.Current.MainPage = new AdminPage();
        }
        async void Confirm(object sender, EventArgs e)
        {
            HttpClient myClient = new HttpClient();
            var uri = new Uri("https://vstorex.com/testmobile/ckuserpop.php?orderkey=" + send.orderkey);
            await myClient.GetAsync(uri);
            await Navigation.PopModalAsync();
            Application.Current.MainPage = new AdminPage();
        }
           
        async void Cancel(object sender, EventArgs e)
		{
            HttpClient myClient = new HttpClient();
            var uri = new Uri("https://vstorex.com/testmobile/ckuserpop.php?orderkey=" + send.orderkey + "&cancel=" + true);
            await myClient.GetAsync(uri);
            await Navigation.PopModalAsync();
            Application.Current.MainPage = new AdminPage();
        }
    }
}