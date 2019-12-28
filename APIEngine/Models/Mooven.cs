using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace APIEngine.Models
{
    public class Mooven
    {
        private string _id;
        private string _region;
        private string _apiKey = "MbtSgJapzT9GGBUDVfiSW2bx8EbVs6M23Yx3GKsi";
        int _retrys = 4;
        public Mooven (string id, string region)
        {
            _id = id;
            _region = region;
        }
        public string GetTravelTime()
        {
            int counter = 0;
            Random random = new Random();
            string response = GetTravelTime1();
            while (response.Length > 10 && counter++ < _retrys)
            {
                Thread.Sleep(random.Next(2, 10) * 1000);
                response = GetTravelTime1();
            }

            return response;

        }
        private string GetTravelTime1()
        {
            string URL = "https://api.mooven.io/v1/"+_region +"/currentjourneytime";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            var response = client.GetAsync(URL);
            try{
                response.Wait();
            }catch(Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task GetAsync Mooven");
                return "Failed internal Error";
            }
            if (!response.Result.IsSuccessStatusCode)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task GetAsync Parkam");
                return "Failed from Mooven Server";
            }
            var r = client.GetStringAsync(URL);
            try
            {
                r.Wait();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task GetStringAsync Mooven");
                return "Failed internal Error";
            }
            var returnedData = r.Result;
            returnedData = returnedData.Remove(1, 44);
            var responseObject = new MoovenTravelTimes();
            var ttList = responseObject.ConvertStr2List(returnedData);
            var tt = ttList.First(x => x.link == _id);
            return tt.travelTime;
        }
    }


    public class MoovenTravelTimes
    {
        public string link { get; set; }
        public string travelTime { get; set; }
        public List<MoovenTravelTimes> ConvertStr2List(string str)
        {
            try
            {
                List<MoovenTravelTimes> ttList = new List<MoovenTravelTimes>();
                var string2Convert = "]" + str;
                var stringS2Convert = string2Convert.Split(']');
                var noOfItems = stringS2Convert.Length - 2;
                for (int i = 1; i < noOfItems; i++)
                {
                    var y = stringS2Convert[i].IndexOf("_");
                    var v = stringS2Convert[i].IndexOf("value");
                    var id = stringS2Convert[i].Substring(y + 1, 4);
                    var val = stringS2Convert[i].Substring(v + 7, stringS2Convert[i].Length - (v + 8));
                    MoovenTravelTimes tt = new MoovenTravelTimes();
                    tt.link = id;
                    tt.travelTime = val;
                    ttList.Add(tt);

                }
                return ttList;
            }catch(Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed to Convert Mooven String - They may have alter their return string");
                return null;
            }
        }
    }


}