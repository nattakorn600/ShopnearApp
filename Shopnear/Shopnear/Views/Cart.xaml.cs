using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Shopnear.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        public ObservableCollection<CartItem> ProductObj;
        public Cart()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ReadDataAsync();
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                cartlabel.Text = "Cart";
                subtotallabel.Text = "Sub Total";
                thblabel.Text = "THB";
                buybut.Text = "Buy";
            }
            else
            {
                cartlabel.Text = "ตะกร้าสินค้า";
                subtotallabel.Text = "รวม ราคา";
                thblabel.Text = "บาท";
                buybut.Text = "สั่งซื้อ";
            }
        }

        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/cart.php?member_id=" + Application.Current.Properties["user_id"]);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<CartItem>>(content);
                ProductObj = new ObservableCollection<CartItem>(Items);
            }
            Caltotal();
        }

        void Caltotal()
		{
            int total = 0;

            for (int i = 0; i < ProductObj.Count; i++)
            {
                total += int.Parse(ProductObj[i].price);
                ProductObj[i].price = string.Format("{0:#,0}", Convert.ToDecimal(ProductObj[i].price));
            }

            Total.Text = string.Format("{0:#,0}", Convert.ToDecimal(total));

            Products.ItemsSource = ProductObj;
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Payment(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Payment());
        }
        async void Neg(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button.BindingContext as CartItem;

            int num = int.Parse(product.number);

            if(num > 1)
            {
                num--;
                product.number = num.ToString();

                var uri = new Uri("https://vstorex.com/testmobile/editcart.php?member_id=" + Application.Current.Properties["user_id"] + 
                    "&product_id=" + product.product_id + "&number=" + product.number);

                HttpClient myClient = new HttpClient();
                await myClient.GetAsync(uri);

                ReadDataAsync();
            }
        }

        async void Posi(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button.BindingContext as CartItem;

            int num = int.Parse(product.number) + 1;
            product.number = num.ToString();

            var uri = new Uri("https://vstorex.com/testmobile/editcart.php?member_id=" + Application.Current.Properties["user_id"] +
                     "&product_id=" + product.product_id + "&number=" + product.number);

            HttpClient myClient = new HttpClient();
            await myClient.GetAsync(uri);

            ReadDataAsync();
        }
        async void DeleteItem(object sender, EventArgs e)
		{
            var button = sender as ImageButton;
            var product = button.BindingContext as CartItem;

            var uri = new Uri("https://vstorex.com/testmobile/deletecart.php?member_id=" + Application.Current.Properties["user_id"] + "&product_id=" + product.product_id);
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);

            ReadDataAsync();
        }
    }
}