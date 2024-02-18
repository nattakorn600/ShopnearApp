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
	public partial class ProductDetail : ContentPage
	{
		public ObservableCollection<ImgProduct> ImgPros;
		public ObservableCollection<follow> follow;

		bool fol_status;
		string text_status;
		Product send = new Product();
		public ProductDetail(Product aProduct)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			send = aProduct;
			ReadDataAsync();
			var uri = new Uri("https://vstorex.com/testmobile/follow.php?mem_id=" + Application.Current.Properties["user_id"]);
			Ck_follow(uri);
			BindingContext = aProduct;
			Number.Text = "1";
			LanguageSwif();
			//DisplayAlert("test", aProduct.id, "OK");
			//enme.Text = TakeProduct.engname;
			//tnme.Text = TakeProduct.thname;
			//pc.Source = TakeProduct.img;
			//st.Text = TakeProduct.story;
		}

		void LanguageSwif()
		{
			if (Application.Current.Properties["language"].ToString() == "eng.png")
			{
				thblabel.Text = "THB";
				detaillabel.Text = "Details";
				addtocart.Text = "Add to Cart +";
			}
			else
			{
				thblabel.Text = "บาท";
				detaillabel.Text = "รายละเอียด";
				addtocart.Text = "เพิ่มเข้าตะกร้า +";
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

		private async void Ck_follow(Uri uri)
		{
			fol_status = false;

			HttpClient myClient = new HttpClient();

			var response = await myClient.GetAsync(uri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var Items = JsonConvert.DeserializeObject<List<follow>>(content);
				follow = new ObservableCollection<follow>(Items);
				for (int i = 0; i < follow.Count; i++)
				{
					if (follow[i].shop_id == send.shop_id)
					{
						fol_status = true;
					}
				}
				if (fol_status == true)
				{
					if (Application.Current.Properties["language"].ToString() == "eng.png")
					{
						btnfollow.Text = "Unfollowing";
					}
					else
					{
						btnfollow.Text = "เลิกติดตาม";
					}
						
					//btnfollow.BackgroundColor = Color.FromHex("d4d7dd");
				}
				else
				{
					if (Application.Current.Properties["language"].ToString() == "eng.png")
					{
						btnfollow.Text = "Following";
					}
					else
					{
						btnfollow.Text = "ติดตาม";
					}
					
					//btnfollow.BackgroundColor = Color.FromHex("404551");
				}
			}
		}

		void BackPage(object sender, EventArgs e)
		{
			//Navigation.PopModalAsync();
			Navigation.PushAsync(new ProductPage());
		}
		void Neg(object sender, EventArgs e)
		{
			if (Number.Text == "")
			{
				Number.Text = "1";
			}
			int number = int.Parse(Number.Text);
			if (number > 1)
			{
				number--;
			}
			Number.Text = number.ToString();
		}
		void Posi(object sender, EventArgs e)
		{
			if (Number.Text == "")
			{
				Number.Text = "0";
			}
			int number = int.Parse(Number.Text);
			number++;
			Number.Text = number.ToString();
		}
		async void Buy(object sender, EventArgs e)
		{
			int number = int.Parse(Number.Text);

			var uri = new Uri("https://vstorex.com/testmobile/addtocart.php?product_id=" + send.product_id + 
				"&member_id=" + Application.Current.Properties["user_id"] + "&number=" + number + "&name=" + send.product_name + 
				"&price=" + send.price + "&img=" + send.img);

			HttpClient myClient = new HttpClient();

			await myClient.GetAsync(uri);

			await Navigation.PushAsync(new Cart());
		}


		private void Pushfollow(object sender, EventArgs e)
		{
			if(fol_status == true)
			{
				text_status = "unfollow";
			}
			else
			{
				text_status = "follow";
			}
			var uri = new Uri("https://vstorex.com/testmobile/follow.php?mem_id=" + Application.Current.Properties["user_id"] + "&shop_id=" + send.shop_id + "&status=" + text_status);
			Ck_follow(uri);
		}
	}
}