using Newtonsoft.Json.Linq;
using PartyOn.model.porfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.profile
{
    public class servicePerfil
    {
        public event EventHandler<PerfilEverArg> GetUserPerfilCompleted;
        public void GetUserPerfil(int id)
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

                   

                    var query = from item in (JArray)doc["dataActivity"]
                                select new modelProfile
                                {
                                    //username = item["username"].Value<string>(),

                                    PhotoPost_UserName = item["PhotoPost_UserName"].Value<string>(),
                                    PhotoPostPhoto = item["PhotoPostPhoto"].Value<string>(),
                                    PhotoPostDescription = item["PhotoPostDescription"].Value<string>(),
                                    PhotoPost_PlaceName = item["PhotoPost_PlaceName"].Value<string>(),
                                    PhotoPostTimeSince = item["PhotoPostTimeSince"].Value<string>(),


                                };

                    var results = query.ToList();


                    if (GetUserPerfilCompleted != null)
                    {
                        GetUserPerfilCompleted(this, new PerfilEverArg(results));
                    }
                }
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));

        }
    }
}
