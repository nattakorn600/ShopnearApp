using Shopnear.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Collections.ObjectModel;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using Plugin.Media;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileInfoPage : ContentPage
    {
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "session.json");
        string _filelang = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "language.json");
        FacebookProfile profile = new FacebookProfile();
        ObservableCollection<CartItem> ProductObj = new ObservableCollection<CartItem>();
        private MediaFile _mediafile;

        public ProfileInfoPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LanguageSwif();
            user_picture.Source = (string)Application.Current.Properties["user_picture"];

            this.BindingContext = profile;
            if(profile.Name==null)
			{
                Name.Text = (string)Application.Current.Properties["user_name"];
            }
            if (profile.Phone == null)
            {
                phone.Text = (string)Application.Current.Properties["user_phone"];
            }
            if (profile.Location == null)
            {
                location.Text = (string)Application.Current.Properties["user_location"];
            }
            if (profile.Account_Number == null)
            {
                account_number.Text = (string)Application.Current.Properties["user_account_number"];
            }
        }
        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                savebut.Text = "Save";
                Name.Placeholder = "Name";
                phone.Placeholder = "Phone Number";
                location.Placeholder = "Location";
                account_number.Placeholder = "Account Number";
            }
            else
            {
                savebut.Text = "บันทึก";
                Name.Placeholder = "ชื่อ";
                phone.Placeholder = "หมายเลขโทรศัพท์";
                location.Placeholder = "ที่อยู่";
                account_number.Placeholder = "หมายเลขบัญชี";
            }
        }
        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private void OnSave(object sender, EventArgs e)
		{
            Upimg();
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


            user_picture.Source = ImageSource.FromStream(() =>
            {
                return _mediafile.GetStream();

            });

        }

        async void UpInfo()
		{
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("id",Application.Current.Properties["user_id"].ToString()),
                    new KeyValuePair<string, string>("name",profile.Name),
                    new KeyValuePair<string, string>("phone",profile.Phone),
                    new KeyValuePair<string, string>("location",profile.Location),
                    new KeyValuePair<string, string>("account_number",profile.Account_Number)
                });
                if (profile.Name != "")
                {
                    var request = await cl.PostAsync("https://vstorex.com/testmobile/editphofile.php?", formcontent);

                    request.EnsureSuccessStatusCode();

                    var response = await request.Content.ReadAsStringAsync();

                    var res = JsonConvert.DeserializeObject<FacebookProfile>(response);

                    string json = JsonConvert.SerializeObject(res, Formatting.Indented);

                    Application.Current.Properties["user_id"] = null;
                    Application.Current.Properties["user_Email"] = null;
                    Application.Current.Properties["user_name"] = null;
                    Application.Current.Properties["user_picture"] = null;
                    Application.Current.Properties["user_phone"] = null;
                    Application.Current.Properties["user_location"] = null;
                    Application.Current.Properties["user_account_number"] = null;
                    Application.Current.Properties["user_status"] = null;
                    File.Delete(_fileName);

                    Application.Current.Properties["user_id"] = res.Id;
                    Application.Current.Properties["user_Email"] = res.Email;
                    Application.Current.Properties["user_name"] = res.Name;
                    Application.Current.Properties["user_picture"] = res.Image;
                    Application.Current.Properties["user_phone"] = res.Phone;
                    Application.Current.Properties["user_location"] = res.Location;
                    Application.Current.Properties["user_account_number"] = res.Account_Number;
                    Application.Current.Properties["user_status"] = res.Status;
                    File.WriteAllText(_fileName, json);

                    await PopupNavigation.Instance.PopAsync();
                    Application.Current.MainPage = new NavigationPage(new ProfilePage());
                }
                else
                {
                    await DisplayAlert("Edit Fail", "Please enter your name", "OK");
                }

            }
        }
        async void Upimg()
        {
            await PopupNavigation.Instance.PushAsync(new LoadingPop());
            WebClient cl = new WebClient();
            if (profile.Name != "")
            {
                if (_mediafile != null)
                {
                    cl.UploadFile("https://vstorex.com/testmobile/editimgpro.php?id="
                        + Application.Current.Properties["user_id"].ToString(), _mediafile.Path);
                }
            }
            UpInfo();
        }
    }
}