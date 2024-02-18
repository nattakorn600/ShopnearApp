using Shopnear;
using Shopnear.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class ProfilePage : ContentPage
    {
        private class StarImg
        {
            public int id { get; set; }
            public string imgsource { get; set; }
        }
        private class StarGet
        {
            public string ratting { get; set; }
            public string star { get; set; }
        }
        List<StarImg> starimg = new List<StarImg>();
        FacebookProfile language = new FacebookProfile();
        public ProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            user_picture.Source = (string)Application.Current.Properties["user_picture"];
            Name.Text = (string)Application.Current.Properties["user_name"];
            languageimg.Source = Application.Current.Properties["language"].ToString();
            LanguageSwif();
            Getstar();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                search.Placeholder = "Search";
                agentcard.Text = "Agent Card";
                sellbut.Text = "Sell";
                historybut.Text = "History";
                followbut.Text = "Following Brands";
                cartbut.Text = "Cart";
                logoutbut.Text = "Logout";
            }
            else
            {
                search.Placeholder = "ค้นหา";
                agentcard.Text = "บัตรตัวแทน";
                sellbut.Text = "ร้านขายของ";
                historybut.Text = "ประวัติการซื้อ";
                followbut.Text = "ร้านที่คุณสนใจ";
                cartbut.Text = "ตะกร้า";
                logoutbut.Text = "ออกจากระบบ";
            }
        }
        void Changelanguage(object sender, EventArgs e)
        {
            string _filelang = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "language.json");
            if (File.Exists(_filelang))
            {
                File.Delete(_filelang);
            }

            if(Application.Current.Properties["language"].ToString() == "eng.png")
            {
                Application.Current.Properties["language"] = "th.png";
            }
            else
            {
                Application.Current.Properties["language"] = "eng.png";
            }

            language.Language = Application.Current.Properties["language"].ToString();
            languageimg.Source = Application.Current.Properties["language"].ToString();
            LanguageSwif();
            string lang = JsonConvert.SerializeObject(language, Formatting.Indented);
            File.WriteAllText(_filelang, lang);
        }
        async void Getstar()
        {
            var uri = new Uri("https://vstorex.com/testmobile/getstar.php?shop_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<StarGet>(content);
                
                if ((Items.ratting == null) || (Items.star == null))
                {
                    Star.Text = string.Format("{0:0.0}", 0.0);
                    starprocess(0);
                }
                else
                {
                    double tud = double.Parse(Items.ratting);
                    Star.Text = string.Format("{0:0.0}", tud);
                    starprocess(int.Parse(Items.star));
                }
            }
        }
        void OnSignoutClick(object sender, EventArgs e)
        {
            string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "session.json");

            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }

            Application.Current.Properties["user_id"] = null;
            Application.Current.Properties["user_Email"] = null;
            Application.Current.Properties["user_name"] = null;
            Application.Current.Properties["user_picture"] = null;
            Application.Current.Properties["user_phone"] = null;
            Application.Current.Properties["user_location"] = null;
            Application.Current.Properties["user_account_number"] = null;
            Application.Current.Properties["user_status"] = null;

            Application.Current.Properties["shop_id"] = null;
            Application.Current.Properties["shop_name"] = null;
            Application.Current.Properties["shop_picture"] = null;
            Application.Current.Properties["shop_agent"] = null;
            Application.Current.Properties["shop_qr"] = null;
            Application.Current.Properties["shop_bank"] = null;
            Application.Current.Properties["shop_account_no"] = null;

            Application.Current.MainPage = new NavigationPage(new loginPage());
        }
        void ToProfileInfoPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfileInfoPage());
        }
        void History(object sender, EventArgs e)
        {
            Navigation.PushAsync(new History());
        }
        void back(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProductPage());
        }
        private async void Addshop(object sender, EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("shop_id",Application.Current.Properties["user_id"].ToString()),
                });

                var request = await cl.PostAsync("https://vstorex.com/testmobile/takeshop.php?", formcontent);

                request.EnsureSuccessStatusCode();

                var response = await request.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<shop>(response);

                if((res.shop_id == null) || (res.shop_id == "No Shop_id"))
				{
                    await Navigation.PushAsync(new AddShop());
				}
                else if((res.map_id == "No Map_id") || (res.map_id == null))
				{
                    await Navigation.PushAsync(new MapPinAdd());
                }
                else
				{
                    Application.Current.Properties["shop_id"] = res.shop_id;
                    Application.Current.Properties["shop_name"] = res.name;
                    Application.Current.Properties["shop_picture"] = res.picture;
                    Application.Current.Properties["shop_agent"] = res.agent_card;
                    Application.Current.Properties["shop_qr"] = res.qr_code;
                    Application.Current.Properties["shop_bank"] = res.bank;
                    Application.Current.Properties["shop_account_no"] = res.account_no;
                    Application.Current.Properties["map_id"] = res.map_id;

                    await Navigation.PushAsync(new ShopPage());

                } 
            }
        }

        private async void Agent(object sender, EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("shop_id",Application.Current.Properties["user_id"].ToString()),
                });

                var request = await cl.PostAsync("https://vstorex.com/testmobile/takeagent.php?", formcontent);

                request.EnsureSuccessStatusCode();

                var response = await request.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<shop>(response);

                if ((res.agent_card == null) || (res.agent_card == "No Agent_Card"))
                {
                    await Navigation.PushAsync(new AddAgentShop());
                }
                else
                {
                    Application.Current.Properties["shop_agent_card"] = res.agent_card;

                    await PopupNavigation.Instance.PushAsync(new AgentCard());
                }
            }
        }

        private async void OnSearch(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new SearchPop(search.Text));
        }

        void FollowBrand(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FollowBrand());
        }

        void Cart(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Cart());
        }
        private void starprocess(int starnum)
        {
            star.ItemsSource = null;
            starimg.Clear();
            for (int i = 0; i < 5; i++)
            {
                if (i < starnum)
                {
                    starimg.Add(new StarImg { id = i + 1, imgsource = "https://vstorex.com/testmobile/white-star.png" });
                }
                else
                {
                    starimg.Add(new StarImg { id = i + 1, imgsource = "https://vstorex.com/testmobile/black-star.png" });
                }
            }
            star.ItemsSource = starimg;
        }
    }
}