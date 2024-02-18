using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        loginpos rg = new loginpos();
        public Register()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = rg;
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                signuplabel.Text = "SIGN UP";
                emaillabel.Placeholder = "Email";
                passlabel.Placeholder = "Password";
                cpwlabel.Placeholder = "Confirm Password";
                signupbut.Text = "SIGN UP";
            }
            else
            {
                signuplabel.Text = "ลงทะเบียน";
                emaillabel.Placeholder = "อีเมล";
                passlabel.Placeholder = "รหัสผ่าน";
                cpwlabel.Placeholder = "ยืนยันรหัสผ่าน";
                signupbut.Text = "ลงทะเบียน";
            }
        }

        async void OnSignUp(object sender, EventArgs e)
		{
            if((rg.email == null) || (rg.pass == null) || (rg.confirmpass == null) || (rg.email == "") || (rg.pass == "") || (rg.confirmpass == ""))
			{
                await DisplayAlert("Alert!", "Please complete all information", "OK");
            }
            else
			{
                if (rg.pass != rg.confirmpass)
                {
                    await DisplayAlert("Alert!", "Passwords do not match", "OK");
                }
                else
				{
                    HttpClient myClient = new HttpClient();
                    var uri = new Uri("https://vstorex.com/testmobile/register.php?email=" + rg.email + "&pass=" + rg.pass);

                    await myClient.GetAsync(uri);
                    await Navigation.PushAsync(new loginPage());
                }
            }
        }
    }
}