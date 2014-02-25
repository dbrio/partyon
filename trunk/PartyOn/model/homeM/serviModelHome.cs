﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.homeM
{
    public class serviModelHome
    {
        public event EventHandler<UserHomeEventArg> GetUserHomeComplete;

        public void GetUserHome()
        {
            string uri = "http://partyonapp.com/API/datahome/?qLat=13.304746993829779&qLong=-87.1910797432877";
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
