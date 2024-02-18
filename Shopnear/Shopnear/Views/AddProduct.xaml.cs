using Shopnear.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        Product proform = new Product();
        int count = 0;
        private List<MediaFile> medialist;// = new List<MediaFile>();
        private class media
		{
            public string mediaPath { get; set; }
		}
        List<media> medialistpath = new List<media>();
        public AddProduct()
        {
            InitializeComponent();
            BindingContext = proform;
            NavigationPage.SetHasNavigationBar(this, false);
            ImgPlus();
            LanguageSwif();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                addproductlabel.Text = "Add Product";
                savebut.Text = "Save";
                clearimgbut.Text = "Clear Image";
                productnamelabel.Text = "Name Product";
                pricelabel.Text = "Price";
                stocklabel.Text = "Stock";
                detaillabel.Text = "Detials";
            }
            else
            {
                addproductlabel.Text = "เพิ่มสินค้า";
                savebut.Text = "บันทึก";
                clearimgbut.Text = "ลบรูปภาพ";
                productnamelabel.Text = "ชื่อสินค้า";
                pricelabel.Text = "ราคา";
                stocklabel.Text = "จำนวน";
                detaillabel.Text = "รายละเอียด";
            }
        }
        private async void AddImage(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Pick Photo", ":(No Pick Photo available.", "ok");
                return;
            }

            //_mediafile = await CrossMedia.Current.PickPhotoAsync();
            medialist = await CrossMedia.Current.PickPhotosAsync();
            
            if (medialist == null)
                return;
            if (medialist != null)
            {
                ProductImg.Children.RemoveAt(count);
            }

            

            for (int i=0; i<medialist.Count; i++)
            {
                medialistpath.Add(new media() { mediaPath = medialist[i].Path });
                Image image = new Image
                {
                    Source = medialist[i].Path,
                    Margin = new Thickness(0, 0, 5, 0),
                    HeightRequest = (125),
                    WidthRequest = (125),
                    BackgroundColor = Color.FromHex("f2f2f2")
                };
                ProductImg.Children.Add(image);
                count++;
            }

            ImgPlus();

        }

        private async void Save(object sender, EventArgs e)
        {
            if ((medialist == null) || (proform.product_name == null) || (proform.product_detail == null) || (proform.price == null) || (proform.stock == null))
            {
                await DisplayAlert("Alert!", "Please complete all information.", "OK");
            }
            else
			{
                await PopupNavigation.Instance.PushAsync(new LoadingPop());
                using (var cla = new HttpClient())
                {
                    var formcontent = new FormUrlEncodedContent(new[]
                {

                    new KeyValuePair<string,string>("shop_id", Application.Current.Properties["shop_id"].ToString()),
                    new KeyValuePair<string, string>("product_name", proform.product_name),
                    new KeyValuePair<string, string>("product_detail", proform.product_detail),
                    new KeyValuePair<string, string>("price", proform.price),
                    new KeyValuePair<string, string>("stock", proform.stock)
                });

                    var request = await cla.PostAsync("https://vstorex.com/testmobile/addproduct.php?", formcontent);

                    request.EnsureSuccessStatusCode();

                    var response = await request.Content.ReadAsStringAsync();

                    var res = JsonConvert.DeserializeObject<Product>(response);

                    WebClient cl = new WebClient();

                    if (medialistpath != null)
                    {
                        for (int i = 0; i < medialistpath.Count; i++)
                        {
                            cl.UploadFile("https://vstorex.com/testmobile/addimgpro.php?shop_id=" + Application.Current.Properties["shop_id"].ToString() + "&product_id=" + res.product_id, medialistpath[i].mediaPath);
                        }
                        medialist = null;
                    }
                }
                await PopupNavigation.Instance.PopAsync();
                Application.Current.MainPage = new NavigationPage(new ProductPage());
            }
        }
        private void ClearImage(object sender, EventArgs e)
		{
            ProductImg.Children.Clear();
            medialistpath.Clear();
            medialist = null;
            count = 0;
            ImgPlus();
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        void ImgPlus()
        {
        ImageButton imgPlus = new ImageButton
            {
                Source = "https://vstorex.com/testmobile/frame.jpg",
                Margin = new Thickness(0, 0, 5, 0),
                HeightRequest = (125),
                WidthRequest = (125),
                BackgroundColor = Color.FromHex("f2f2f2")
            };
            ProductImg.Children.Add(imgPlus);
            imgPlus.Clicked += AddImage;

            /*if (medialist == null)
            {
                ProductImg.Children.Clear();
                ImageButton imgPlus = new ImageButton
                {
                    Source = "https://vstorex.com/testmobile/frame.jpg",
                    Margin = new Thickness(0, 0, 5, 0),
                    HeightRequest = (125),
                    WidthRequest = (125)
                };
                ProductImg.Children.Add(imgPlus);
                imgPlus.Clicked += AddImage;
            }
            else
			{
                ProductImg.Children.RemoveAt(0);
            }*/
        }
    }
}