using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shopnear.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
	public partial class ShopProductDetail : ContentPage
	{
		public ObservableCollection<ImgProduct> ImgPros;
		public ObservableCollection<follow> follow;

		Product send = new Product();
		public ShopProductDetail(Product aProduct)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			send = aProduct;
			ReadDataAsync();
			var uri = new Uri("https://vstorex.com/testmobile/follow.php?mem_id=" + Application.Current.Properties["user_id"]);
			BindingContext = aProduct;
			LanguageSwif();
		}

		void LanguageSwif()
		{
			if (Application.Current.Properties["language"].ToString() == "eng.png")
			{
				thblabel.Text = "THB";
				detaillabel.Text = "Details";
				dellabel.Text = "Delete this product";
			}
			else
			{
				thblabel.Text = "บาท";
				detaillabel.Text = "รายละเอียด";
				dellabel.Text = "ลบสินค้านี้";
			}
		}
		private async void ReadDataAsync()
		{
			var uri = new Uri("https://vstorex.com/testmobile/imgpro.php?product_id=" + send.product_id);
			HttpClient myClient = new HttpClient();

			var response = await myClient.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var Items = JsonConvert.DeserializeObject<List<ImgProduct>>(content);
				ImgPros = new ObservableCollection<ImgProduct>(Items);
				Products.ItemsSource = ImgPros;
			}
		}

		void BackPage(object sender, EventArgs e)
		{
			//Navigation.PopModalAsync();
			Navigation.PushAsync(new ShopPage());
		}
		
		async void Del(object sender, EventArgs e)
		{
			var uri = new Uri("https://vstorex.com/testmobile/deleteproduct.php?product_id=" + send.product_id + 
				"&shop_id=" + Application.Current.Properties["user_id"]);

			HttpClient myClient = new HttpClient();

			await myClient.GetAsync(uri);

			Application.Current.MainPage = new NavigationPage(new ProductPage());
		}
	}
}