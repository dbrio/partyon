﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.song
{
    public class ServiceSong
    {
        public event EventHandler<UserSongEverArg> GetUserSongCompleted;
        public void GetUserSong()
        {
            string uri = "http://www.partyonapp.com/API/heydj/?qLat=13.309102541101671&qLong=-87.18679750841795";

            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, a) =>
            {
                if (a.Error == null && !a.Cancelled)
                {
                    var result = a.Result;

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
            };
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));

        }
    }
}
