using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PartyOn.model
{
    public class ServiceModel
    {
        public event EventHandler<UserActivityEventArgs> GetUserActivityCompleted;
        public async void GetUserActivity()
        {
            string uri = "http://partyonapp.com/API/dataactivity/";
            HttpClient client = new HttpClient();
            var result = await client.GetStringAsync(uri);


            //serialize Activity

            var doc = JObject.Parse(result);

            var query = from item in (JArray)doc["data"]
                        select new modelActivity
                        {
                            PhotoPost_UserName = item["PhotoPost_UserName"].Value<string>(),
                            PhotoPostDescription = item["PhotoPostDescription"].Value<string>(),
                            PhotoPost_PlaceName = item["PhotoPost_PlaceName"].Value<string>(),
                            PhotoPostDateTime = item["PhotoPostDateTime"].Value<string>(),
                            PhotoPostPhoto = item["PhotoPostPhoto"].Value<string>(),
                            PhotoPost_UserAvatar =item["PhotoPost_UserAvatar"].Value<string>(),
                            PhotoPostTimeSince = item["PhotoPostTimeSince"].Value<string>(),
                        };


            var results = query.ToList();

            if (GetUserActivityCompleted != null)
            {
                GetUserActivityCompleted(this, new UserActivityEventArgs(results));
            }
        }
            
            

    }

}
