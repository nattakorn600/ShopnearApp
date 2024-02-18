using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : Shell
    {
        public AdminPage()
        {
            InitializeComponent();
            //LanguageSwif();
        }

   /*     void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                adminlabel.Text = "Admin";
                userlabel.Text = "Check order";
                shoplabel.Text = "Pay to shop";
                logoutlabel.Text = "Logout";
            }
            else
            {
                adminlabel.Text = "แอดมิน";
                userlabel.Text = "ตรวจสอบรายการ";
                shoplabel.Text = "จ่ายเงินให้ร้านค้า";
                logoutlabel.Text = "ออกจากระบบ";
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

            App.Current.MainPage = new loginPage();
        }
        void User(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CkUser());
        }
        void Shop(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CkShop());
        } */
    }
}