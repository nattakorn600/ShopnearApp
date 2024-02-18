using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddShop : ContentPage
    {
        public AddShop()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            user_picture.Source = (string)Application.Current.Properties["user_picture"];
            Name.Text = (string)Application.Current.Properties["user_name"];
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                addbut.Text = "+Add";
            }
            else
            {
                addbut.Text = "+เพิ่ม";
            }
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        void Addhopdetail(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddNameShop());

        }
    }
}