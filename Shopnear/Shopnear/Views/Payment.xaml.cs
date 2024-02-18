using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payment : ContentPage
    {
        ObservableCollection<CartItem> ProductObj = new ObservableCollection<CartItem>();
        private MediaFile _mediafile;
        public Payment()
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
                paymentlabel.Text = "Payment";
                ordertolabel.Text = "Order Total";
                itemlabel.Text = "item";
                totalpaylabel.Text = "Total Payment";
                thblabel.Text = "THB";
                accountnumlabel.Text = "Account Number";
                addpaylabel.Text = "Add Your Payment";
                finishbut.Text = "Finish";
            }
            else
            {
                paymentlabel.Text = "การชำระเงิน";
                ordertolabel.Text = "รายการวม";
                itemlabel.Text = "ชิ้น";
                totalpaylabel.Text = "เงินที่ต้องชำระรวม";
                thblabel.Text = "บาท";
                accountnumlabel.Text = "หมายเลขบัญชี";
                addpaylabel.Text = "หลักฐานการชำระเงิน";
                finishbut.Text = "เสร็จสิ้น";
            }
        }

        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/cart.php?member_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<CartItem>>(content);
                ProductObj = new ObservableCollection<CartItem>(Items);
            }
            Caltotal();
        }

        void Caltotal()
        {
            int total = 0;
            int totalorder = 0;

            for (int i = 0; i < ProductObj.Count; i++)
            {
                total += int.Parse(ProductObj[i].price);
                totalorder += int.Parse(ProductObj[i].number);
            }

            TotalPrice.Text = string.Format("{0:#,00}", Convert.ToDecimal(total));
            TotalOrder.Text = string.Format("{0:#,0}", Convert.ToDecimal(totalorder));
        }

        private async void ImageClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Pick Photo", ":(No Pick Photo available.", "ok");
                return;
            }

            //_mediafile = await CrossMedia.Current.PickPhotoAsync();
            _mediafile = await CrossMedia.Current.PickPhotoAsync();

            if (_mediafile == null)
                return;


            ShopImage.Source = ImageSource.FromStream(() =>
            {
                return _mediafile.GetStream();

            });

        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void Send(object sender, EventArgs e)
		{
            if (_mediafile == null)
            {
                await DisplayAlert("Alert!", "Please complete all information.", "OK");
            }
			else 
            {
                var ran = new Random();
                string orderkey = ran.Next(0, 9999999).ToString() + ran.Next(0, 9999999).ToString();

                HttpClient myClient = new HttpClient();

                for (int i = 0; i < ProductObj.Count; i++)
                {
                    var uri = new Uri("https://vstorex.com/testmobile/addhistory.php?mem_id=" + Application.Current.Properties["user_id"] +
                       "&product_id=" + ProductObj[i].product_id + "&number=" + ProductObj[i].number + "&orderkey=" + orderkey);

                    await myClient.GetAsync(uri);
                }

                Upimg(orderkey);
            }
        }
        async void Upimg(string orderkey)
		{
            WebClient cl = new WebClient();

            await PopupNavigation.Instance.PushAsync(new LoadingPop());

            cl.UploadFile("https://vstorex.com/testmobile/addimghis.php?mem_id="
                + Application.Current.Properties["user_id"].ToString() + "&orderkey=" + orderkey, _mediafile.Path);

            ShopImage.Source = null;
            await PopupNavigation.Instance.PopAsync();
            Application.Current.MainPage = new NavigationPage(new ProductPage());
        }
    }
}