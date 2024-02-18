using Newtonsoft.Json;
using Plugin.Geolocator;
using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        public ObservableCollection<MapPin> mappin;

        public ObservableCollection<CustomPin> pin = new ObservableCollection<CustomPin>();
        public MapPage()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            customMap.CustomPins = new List<CustomPin>();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(13.7543822883747, 100.500995479524), Distance.FromMiles(500)));
            LanguageSwif();
            ReadDataAsync();
            PositionUser();
        }
        void LanguageSwif()
        {
            if (Application.Current.Properties["language"].ToString() == "eng.png")
            {
                maplabel.Text = "Map";
            }
            else
            {
                maplabel.Text = "แผนที่";
            }
        }
        public async void ReadDataAsync()
        {
            var uri = new Uri("https://vstorex.com/testmobile/map.php");
            HttpClient myClient = new HttpClient();

            var response = await myClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<MapPin>>(content);
                //mappin = new ObservableCollection<MapPin>(Items);
                Pin(Items);
            }
        }
       
       void Pin(List<MapPin>  mappin)
		{
            for (int i = 0; i < mappin.Count; i++)
			{
                pin.Add(new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(Double.Parse(mappin[i].latitude), Double.Parse(mappin[i].longtitude)),
                    Label = mappin[i].name,
                    Address = mappin[i].address,
                    Url = mappin[i].shop_id,
                    Source = mappin[i].img
                });

                customMap.Pins.Add(pin[i]);
                customMap.CustomPins.Add(pin[i]);
            }
        }
        async void PositionUser()
		{
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var position = await Geolocation.GetLocationAsync(request);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(200)));        
        }

        void BackPage(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}