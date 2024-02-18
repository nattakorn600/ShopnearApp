using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shopnear;
using Shopnear.Models;
using Shopnear.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Shopnear.Services
{
    public class FacebookServices
    {
        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "session.json");
        public async Task<FacebookProfile> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages&access_token="
                + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);
            facebookProfile.Image = "";

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string,string>("id",facebookProfile.Id),
                    new KeyValuePair<string, string>("name",facebookProfile.Name),
                    new KeyValuePair<string, string>("pic",facebookProfile.Picture.Data.Url)
                });

                var request = await cl.PostAsync("https://vstorex.com/testmobile/loginface.php?", formcontent);

                request.EnsureSuccessStatusCode();

                var response = await request.Content.ReadAsStringAsync();

                var res = JsonConvert.DeserializeObject<FacebookProfile>(response);

                string json = JsonConvert.SerializeObject(res, Formatting.Indented);
                File.WriteAllText(_fileName, json);

                Application.Current.Properties["user_id"] = res.Id;
                Application.Current.Properties["user_name"] = res.Name;
                Application.Current.Properties["user_email"] = res.Email;
                Application.Current.Properties["user_picture"] = res.Image;
                Application.Current.Properties["user_phone"] = res.Phone;
                Application.Current.Properties["user_location"] = res.Location;
                Application.Current.Properties["user_account_number"] = res.Account_Number;
                Application.Current.Properties["user_status"] = res.Status;

                return facebookProfile;
            }
        }
    }
}
