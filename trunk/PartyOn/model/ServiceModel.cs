using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PartyOn.model
{
    public class ServiceModel
    {
        public event EventHandler<UserActivityEventArgs> GetUserActivityCompleted;
        public void GetUserActivity()
        {
            string uri = "http://partyonapp.com/API/dataactivity/";
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

                    //LlNQ 

                    var doc = JObject.Parse(result);

                    var query = from item in (JArray)doc["data"]
                                select new modelActivity
                                {
                                    PhotoPost_UserName = item["PhotoPost_UserName"].Value<string>(),
                                    PhotoPostDescription = item["PhotoPostDescription"].Value<string>(),
                                    PhotoPost_PlaceName = item["PhotoPost_PlaceName"].Value<string>(),
                                };
                    var results = query.ToList();

                    if(GetUserActivityCompleted != null)
                    {
                        GetUserActivityCompleted(this, new UserActivityEventArgs(results));
                    }
                }
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
            
        }
    }

}
