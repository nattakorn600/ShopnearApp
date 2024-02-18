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
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNameShop : ContentPage
    {
        private List<MediaFile> medialist = new List<MediaFile>();
        private MediaFile _mediafile;
        private shop shop = new shop();
        public AddNameShop()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = shop;
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                addshopimgbut.Text = "Add shop picture";
                shopnamelabel.Text = "Shop name";
                backbut.Text = "back";
                nextbut.Text = "next";
            }
            else
            {
                addshopimgbut.Text = "เพิ่มรูปร้าน";
                shopnamelabel.Text = "ชื่อร้าน";
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

        private async void NextPage(object sender, EventArgs e)
        {
            WebClient cl = new WebClient();
            //DisplayAlert("test",medialist.Count.ToString(),"OK");
            if((shop.name == null) || (_mediafile == null))
			{
                await DisplayAlert("Alert!", "Please complete all information.", "OK");
			}
            else
			{
                await PopupNavigation.Instance.PushAsync(new LoadingPop());
                cl.UploadFile("https://vstorex.com/testmobile/addshop.php?shop_id=" 
                    + Application.Current.Properties["user_id"].ToString() + "&shop_name=" 
                    + shop.name, _mediafile.Path);
                ShopImage.Source = null;
                await PopupNavigation.Instance.PopAsync();
                await Navigation.PushAsync(new AddPayShop());
            }  
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}