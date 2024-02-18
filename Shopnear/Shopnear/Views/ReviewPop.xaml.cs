using Rg.Plugins.Popup.Services;
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
    public partial class ReviewPop 
    {
		string starnub;
        private class StarImg
        {
            public int id { get; set; }
            public string imgsource { get; set; }
        }

		string key;
        List<StarImg> starimg = new List<StarImg>();

        public ReviewPop(string orderkey)
        {
            InitializeComponent();
			key = orderkey;
			starprocess(0);
			LanguageSwif();
		}

		void LanguageSwif()
		{
			if (Application.Current.Properties["language"].ToString() == "eng.png")
			{
				reviewlabel.Text = "Review";
				comlabel.Text = "Comment";
				sendbut.Text = "Send";
			}
			else
			{
				reviewlabel.Text = "ให้คะแนน";
				comlabel.Text = "ความคิดเห็น";
				sendbut.Text = "ส่ง";
			}
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
		void OnStarClick(object sender, SelectionChangedEventArgs e)
		{
			var selectproduct = (e.CurrentSelection.FirstOrDefault() as StarImg).id;
			starnub = selectproduct.ToString();
			starprocess(selectproduct);
		}
		async void Send(object sender, EventArgs e)
		{
			HttpClient myClient = new HttpClient();
			var uri = new Uri("https://vstorex.com/testmobile/reviewset.php?orderkey=" + key + "&mem_id=" + 
				Application.Current.Properties["user_id"] + "&star=" + starnub + "&comment=" + Comment.Text);
			await myClient.GetAsync(uri);
			await PopupNavigation.Instance.PopAsync();
		}
	}
}