using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using PartyOn;

namespace PartyOn.model.getPlace
{
    public class serviceModelPlace
    {
        public event EventHandler<UserPlaceEvertArg> GetUserPlaceComplete;

         public void GetUserPlace(double lat, double longi)
        {
            

            string uriJson = "http://partyonapp.com/API/getplaces/";

            string uri = uriJson + "?qLat=" + lat + "&qLong=" + longi;

            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

                    //serialize Home

                    var doc = JObject.Parse(result);

                    var query = from item in (JArray)doc["data"]
                                select new modelPlace
                                {
                                    PlaceName = item["PlaceName"].Value<string>(),
                                    PlaceID = item["PlaceID"].Value<int>(),
                                };
                    var results = query.ToList();

                    if (GetUserPlaceComplete != null)
                    {
                        GetUserPlaceComplete(this, new UserPlaceEvertArg(results));
                    }
                }
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
        }
    }
}
