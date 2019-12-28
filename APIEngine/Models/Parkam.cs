using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace APIEngine.Models
{
    public class Parkam
    {
        string _token = "73267347a10a368b347a704f46d7436bdff3153c";
        string _type;
        string _siteID;
        int _retrys = 4;
        public Parkam(string type, string siteID)
        {
            _type = type;
            _siteID = siteID;
        }
        public string GetCurrentVacant()
        {
            int counter = 0;
            Random random = new Random();
            string response = GetValue();
            while (response.Length > 10  && counter++ < _retrys)
            {
                Thread.Sleep(random.Next(2, 10) * 1000);
                response = GetValue();
            }

            return response;
        }
 
        private string GetValue()
        {
            string URL = "https://web.parkam-ip.com/api/occupancy/text/parkingTotalVacant/" + _type + "/" + _siteID + "/";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            client.DefaultRequestHeaders.Add("X-Auth-Token", _token);
            var response = client.GetAsync(URL);
            try
            {
                response.Wait();
            }catch(Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task GetAsync Parkam");
                return "Internal API Engine Error";
            }
            if (!response.Result.IsSuccessStatusCode)
            {
                EventLog.WriteEntry("APIEngine", "Error from Parkam Server "+response.Result.ToString());
                return "Error from Parkam Server ";
            }
            var r = client.GetStringAsync(URL);
            try
            {
                r.Wait();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task GetStringAsync Parkam");
                return "Internal API Engine Error";
            }
            r.Wait();
            return r.Result;
        }
     }
}