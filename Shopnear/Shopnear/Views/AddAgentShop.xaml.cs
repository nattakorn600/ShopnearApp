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
    public partial class AddAgentShop : ContentPage
    {
        private MediaFile _mediafile;
        public AddAgentShop()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LanguageSwif();
        }
        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                agentlabelbut.Text = "Add Agent Card";
                backbut.Text = "back";
                savebut.Text = "save";
            }
            else
            {
                agentlabelbut.Text = "เพิ่มบัตรตัวแทน";
                backbut.Text = "ย้อนกลับ";
                savebut.Text = "บันทึก";
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
            if (_mediafile == null)
            {
                await DisplayAlert("Alert!", "Please complete all information.", "OK");
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new LoadingPop());
                cl.UploadFile("https://vstorex.com/testmobile/addagentcard.php?shop_id="
                    + Application.Current.Properties["user_id"].ToString(), _mediafile.Path);
                ShopImage.Source = null;
                await PopupNavigation.Instance.PopAsync();
                await Navigation.PopAsync();
            }
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }


}