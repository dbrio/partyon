using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.song
{
    public class ServiceSong
    {
        public event EventHandler<UserSongEverArg> GetUserSongCompleted;
        public async void GetUserSong(double lati, double longi)
        {
            string uriweb = "http://www.partyonapp.com/API/heydj/";
            string uri = uriweb + "?qLat=" + lati + "&qLong=" + longi; 

            HttpClient client = new HttpClient();
            
                    var result = await client.GetStringAsync(uri);

                    //serialize Activity

                    var doc = JObject.Parse(result);

                    var query = from item in (JArray)doc["data"]
                                select new modelSong
                                {
                                    SongPostName = item["SongPostName"].Value<string>(),
                                    SongPostQuote = item["SongPostQuote"].Value<string>(),
                                    UserFirstName = item["UserFirstName"].Value<string>(),
                                    Username = item["Username"].Value<string>(),
                                };
                    var results = query.ToList();

                    if (GetUserSongCompleted != null)
                    {
                        GetUserSongCompleted(this, new UserSongEverArg(results));
                    }
              
        }
    }
}
