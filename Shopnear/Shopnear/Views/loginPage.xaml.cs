using Shopnear;
using Shopnear.Services;
using Shopnear.Models;
using Shopnear.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class loginPage : ContentPage
    {
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "session.json");
        loginpos lg = new loginpos();
        private string ClientId = "285329936035493";
        public loginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = lg;
            lblClickSignUp();
        }
        private async void OnLoginClick(object sender, EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string,string>("email",lg.email),
            new KeyValuePair<string, string>("pass",lg.pass)
        });


                var request = await cl.PostAsync("https://vstorex.com/testmobile/ck_login.php?", formcontent);

                request.EnsureSuccessStatusCode();

                var response = await request.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<FacebookProfile>(response);

                //await DisplayAlert("Alert", status +" "+ usid, "OK") ;
                if ((res.Image == "") || (res.Image == null))
                {
                    res.Image = "https://vstorex.com/testmobile/profile.png";
                }

                if ((res.Id != "password fail") && (res.Id != "username fail"))
                {
                    string json = JsonConvert.SerializeObject(res, Formatting.Indented);
                    File.WriteAllText(_fileName, json);

                    Application.Current.Properties["user_id"] = res.Id;
                    Application.Current.Properties["user_Email"] = res.Email;
                    Application.Current.Properties["user_name"] = res.Name;
                    Application.Current.Properties["user_picture"] = res.Image;
                    Application.Current.Properties["user_phone"] = res.Phone;
                    Application.Current.Properties["user_location"] = res.Location;
                    Application.Current.Properties["user_account_number"] = res.Account_Number;
                    Application.Current.Properties["user_status"] = res.Status;

                    //await Navigation.PushModalAsync(new Page1());
                    if(res.Status == "admin")
					{
                        App.Current.MainPage = new AdminPage();
                    }
                    else
					{
                        App.Current.MainPage = new NavigationPage(new ProductPage());
                    }   
                }
                else
                {
                    if(res.Id == "username fail")
                    {
                        await DisplayAlert("login failed", "Invalid email", "OK");
                    }
                    else
                    {
                        await DisplayAlert("login failed", "Incorrect password", "OK");
                    }
                    
                }

            }
        }
        private void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new FacebookProfilePage());
            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;

        }
        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);
            int pas = 0;

            if (accessToken != "")
            {
                var vm = new FacebookServices();

                await vm.GetFacebookProfileAsync(accessToken);

                Content = MainStackLayout;

                pas += 1;
            }
            if (pas > 0)
            {
                pas = 0;
                App.Current.MainPage = new NavigationPage(new ProductPage());
            }
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");
                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }
        void lblClickSignUp()
        {
            lblClickSU.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new Register());
                })
            });
        }
    }
}
