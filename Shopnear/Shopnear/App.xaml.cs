using Shopnear.Models;
using Shopnear.Views;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

namespace Shopnear
{
    public partial class App : Application
    {
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "session.json");
        string _filelang = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "language.json");

        public App()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Device.SetFlags(new string[] { "CarouselView_Experimental", "MediaElement_Experimental", "SwipeView_Experimental","IndicatorView_Experimental" });
            /*if (File.Exists(_fileName))
             {
                 File.Delete(_fileName);
             }
             App.Current.MainPage = new loginPage();*/
        }

        protected override void OnStart()
        {
            if(File.Exists(_filelang))
            {
                FacebookProfile lang = new FacebookProfile();
                string jsonlang = File.ReadAllText(_filelang);
                lang = JsonConvert.DeserializeObject<FacebookProfile>(jsonlang);

                Application.Current.Properties["language"] = lang.Language;
            }
            else
            {
                Application.Current.Properties["language"] = "eng.png";
            }

            if (!(File.Exists(_fileName)))
            {
                MainPage = new NavigationPage(new loginPage());
                //File.Create(session).Dispose();
            }
            else
            {
                FacebookProfile data = new FacebookProfile();
                string jsondata = File.ReadAllText(_fileName);
                data = JsonConvert.DeserializeObject<FacebookProfile>(jsondata);

                if ((data.Id != "") && (data.Id != "Username fail") && (data.Id != "Password fail"))
                {
                    Application.Current.Properties["user_id"] = data.Id;
                    Application.Current.Properties["user_Email"] = data.Id;
                    Application.Current.Properties["user_name"] = data.Name;
                    Application.Current.Properties["user_phone"] = data.Phone;
                    Application.Current.Properties["user_location"] = data.Location;
                    Application.Current.Properties["user_account_number"] = data.Account_Number;
                    Application.Current.Properties["user_status"] = data.Status;

                }
                if (data.Image != "")
                {
                    Application.Current.Properties["user_picture"] = data.Image;
                }
                else
                {
                    Application.Current.Properties["user_picture"] = data.Picture.Data.Url;
                }

                if (data.Status == "admin")
                {
                    MainPage = new AdminPage();
                }
                else
                {
                    MainPage = new NavigationPage(new ProductPage());
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
