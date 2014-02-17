using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model
{
    class ServiceModel
    {
        public event EventHandler<UserActivityEventArgs> GetUserActivityCompleted;

        public void GetUserActivity()
        {
            string uri = "http://partyonapp.com/API/dataactivity/";
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri(uri, UriKind.Absolute));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                try
                {
                    //var root1 = JsonConvert.DeserializeObject<modelActivity>(e.Result);
                    //var obj = JObject.Parse(e.Result);
                    //var query = from c in obj["data"].Children["PhotoPost_UserFirstName"].Value<string>()
                    //            select new modelActivity() { UserFirstName = c.ElementAt("PhotoPost_UserFirstName").Value };
                    //var results = query.ToList();

                    JObject objResult = JObject.Parse(e.Result);
                    //IList<modelActivity> results = objResult.Select(p => new modelActivity
                    //{
                    //    UserFirstName = (string)p["PhotoPost_UserFirstName"]
                    //}).ToList();
                    //IList<modelActivity> results = new IList<modelActivity>;

                    //results.Add(new modelActivity(){ UserFirstName = "Mocos"});

                    JArray a = (JArray)objResult["data"];

                    IList<modelActivity> results = a.ToObject<IList<modelActivity>>();

                    if (GetUserActivityCompleted != null)
                    {
                        GetUserActivityCompleted(this, new UserActivityEventArgs(results));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
