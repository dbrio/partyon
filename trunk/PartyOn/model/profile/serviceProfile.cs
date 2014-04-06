using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.porfile
{
    public class serviceProfile
    {
        public event EventHandler<UserPorfileEventArg> GetUserProfileCompleted;
        public void GetUserProfile( int id)
        {
            string uriweb = "http://www.partyonapp.com/API/userprofile/?uid=";
            string uri = uriweb + id;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

                    //serialize Activity

                    var doc = JObject.Parse(result);

                    var query = from item in (JArray)doc["data"]
                                select new modelProfile
                                {
                                    username = item["username"].Value<string>(),
                                    ultFotoTomada = item["ultFotoTomada"].Value<string>(),
                                    first_name = item["first_name"].Value<string>(),
                                    last_name = item["last_name"].Value<string>(),
                                    photo = item["photo"].Value<string>(),

                                };

                    

                    var results = query.ToList();


                    if (GetUserProfileCompleted != null)
                    {
                        GetUserProfileCompleted(this, new UserPorfileEventArg(results));
                    }
                }
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));

        }
    }
}
