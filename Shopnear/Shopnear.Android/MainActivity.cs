using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Xamarin.Forms;
using Android.Content;

namespace Shopnear.Droid
{
    [Activity(Label = "Shopnear", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation , ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            //Xamarin.FormsMaps.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            turnOnGps();
            LoadApplication(new App());
        }

        
		protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    Console.WriteLine("Location permissions already granted.");
                }
            }
        }
       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    Console.WriteLine("Location permissions granted.");
                }
                else
                {
                    Console.WriteLine("Location permissions denied.");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
        public async void turnOnGps()
        {
            MainActivity activity = Forms.Context as MainActivity;

            GoogleApiClient googleApiClient = new GoogleApiClient.Builder(activity)
                .AddApi(LocationServices.API).Build();
            googleApiClient.Connect();
            LocationRequest locationRequest = LocationRequest.Create();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(10000 / 2);

            LocationSettingsRequest.Builder
                    locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                    .AddLocationRequest(locationRequest);
            locationSettingsRequestBuilder.SetAlwaysShow(false);
            LocationSettingsResult locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                googleApiClient, locationSettingsRequestBuilder.Build());

            if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
            {
                locationSettingsResult.Status.StartResolutionForResult(activity, 0);
            }
        }
    }
}