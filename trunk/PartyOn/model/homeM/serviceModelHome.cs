using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace PartyOn.model.homeM
{
    public class serviceModelHome
    {
        public event EventHandler<UserHomeEventArg> GetUserHomeComplete;

         public void GetUserHome()
        {
            MainPage geoLocation = new MainPage();

            var PlaceLat = geoLocation.PlaceLat;
            var PlaceLong = geoLocation.PlaceLong;

            string uriJson = "http://partyonapp.com/API/datahome/";

            string uri =  uriJson +"?qLat="+ PlaceLat +"&qLong="+ PlaceLong; 

            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
                {
                   if (a.Error == null && !a.Cancelled)
                   {
                       var result = a.Result;

                       //serialize Home

                       var doc = JObject.Parse(result);

                       var query = from item in (JArray)doc["data"]
                                   select new modelHome
                                   {
                                       PlaceName = item["PlaceName"].Value<string>(),
                                       LastPhoto = item["LastPhoto"].Value<string>(),
                                       PeopleNow = item["PeopleNow"].Value<int>(),
                                       LastPostDate = item["LastPostDate"].Value<string>(),

                                   };
                       var results = query.ToList();

                       if (GetUserHomeComplete != null)
                       {
                           GetUserHomeComplete(this, new UserHomeEventArg(results));
                       }
                   }
                };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
        }
    }
}
