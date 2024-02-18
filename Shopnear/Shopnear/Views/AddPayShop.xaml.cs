using Shopnear.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPayShop : ContentPage
    {
        private MediaFile _mediafile;
        private shop shop = new shop();
        public AddPayShop()
        {
            InitializeComponent();
            BindingContext = shop;
            NavigationPage.SetHasNavigationBar(this, false);
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                qrlabel.Text = "Add the QR code on your bank";
                banklabel.Text = "Bank Name";
                accountlabel.Text = "Account No";
                backbut.Text = "back";
                nextbut.Text = "next";
            }
            else
            {
                qrlabel.Text = "เพิ่ม QR code ธนาคารของคุณ";
                banklabel.Text = "ชื่อธนาคาร";
                accountlabel.Text = "หมายเลขบัญชี";
                backbut.Text = "ย้อนกลับ";
                nextbut.Text = "ถัดไป";
            }
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

        private async void Create(object sender, EventArgs e)
        {
            WebClient cl = new WebClient();
            //DisplayAlert("test",medialist.Count.ToString(),"OK");
            if ((shop.account_no == null) || (_mediafile == null) || (shop.bank == null))
            {
                await DisplayAlert("Alert!", "Please complete all information.", "OK");
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new LoadingPop());
                cl.UploadFile("https://vstorex.com/testmobile/addpay.php?shop_id="
                    + Application.Current.Properties["user_id"].ToString() + "&bank="
                    + shop.bank + "&account_no="
                    + shop.account_no, _mediafile.Path);
                ShopImage.Source = null;
                await PopupNavigation.Instance.PopAsync();
                await Navigation.PushAsync(new MapPinAdd());
            }
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}