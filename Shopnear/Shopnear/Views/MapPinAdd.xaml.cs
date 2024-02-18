using Plugin.Geolocator;
using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using System.Net;
using Xamarin.Forms.PlatformConfiguration;
using System.Net.Http;

namespace Shopnear.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPinAdd : ContentPage
	{
        bool FirstclickMap = true;
        double Latitude = 0;
        double Longitude = 0;
        public MapPinAdd()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(13.7543822883747, 100.500995479524), Distance.FromMiles(500)));
            LanguageSwif();
            PositionUser();
        }

        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                addmarklabel.Text = "Add Marker";
                shopaddresslabel.Text = "Shop Address";
                chooseLocationButton.Text = "Confirm location";
            }
            else
            {
                addmarklabel.Text = "เพิ่มตำแหน่งร้าน";
                shopaddresslabel.Text = "ที่อยู่ร้าน";
                chooseLocationButton.Text = "ยืนยันตำแหน่ง";
            }
        }

        async void PositionUser()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var position = await Geolocation.GetLocationAsync(request);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(200)));
        }

        void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            //Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
            //DisplayAlert("MapClick", $"MapClick: {e.Position.Latitude}, {e.Position.Longitude}", "OK");
            Latitude = e.Position.Latitude;
            Longitude = e.Position.Longitude;

            Pin pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(Latitude, Longitude),
                Label = "Your Shop Location",
                Address = ""
            };
           
            if (FirstclickMap)
			{
                customMap.Pins.Add(pin);
                FirstclickMap = false;
            }
            else
			{
                customMap.Pins.Clear();
                customMap.Pins.Add(pin);
            }
        }

        private async void Pickup(object sender, EventArgs e)
        {
            if((Latitude != 0) && (Longitude != 0) && (Address.Text != null) && (Address.Text != ""))
			{
                var uri = new Uri("https://vstorex.com/testmobile/addmap.php?mem_id=" + Application.Current.Properties["user_id"] + "&latitude=" + Latitude +
                    "&longtitude=" + Longitude + "&address=" + Address.Text);
                HttpClient myClient = new HttpClient();

                var response = await myClient.GetAsync(uri);
                Application.Current.MainPage = new NavigationPage(new ProfilePage());
            }
            else
			{
                await DisplayAlert("Alert!", "Please complete all information.", "OK");
            }
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}