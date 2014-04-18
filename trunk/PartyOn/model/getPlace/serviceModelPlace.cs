using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using PartyOn;
using System.Net.Http;

namespace PartyOn.model.getPlace
{
    public class serviceModelPlace
    {
        public event EventHandler<UserPlaceEvertArg> GetUserPlaceComplete;

         public async void GetUserPlace(double lat, double longi)
        {
            

            string uriJson = "http://partyonapp.com/API/getplaces/";

            string uri = uriJson + "?qLat=" + lat + "&qLong=" + longi;

            HttpClient client = new HttpClient();

            var result = await client.GetStringAsync(uri);

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
           
    }
}
