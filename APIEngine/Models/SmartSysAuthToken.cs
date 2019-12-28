using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace APIEngine.Models
{
    public static class SmartSysAuthToken
    {
        private static readonly string ApiKey = "9jNwgogyARHAfKDwPIUO3icWo2ExyC1C";
        private static readonly string ApiSecret = "iG9SYdEoVEUXpbmR";
        private static ExpirableToken token {get; set;}
        static readonly object _lockObject = new object();

        private static ExpirableToken GetNewToken()
        {
            token = new ExpirableToken();
            token.token = GetTokenFromApi(); 
            token.expiryTime = DateTime.Now.AddMinutes(59);
            return token;
        }
        public static ExpirableToken GetToken()
        {
            lock (_lockObject)
            {
                if (token == null || token.token == "Failed" || token.expiryTime < DateTime.Now)
                {
                    return GetNewToken();
                }
                else
                {
                    return token;
                }
            }

        }
        private static string GetTokenFromApi() 
        {
            string URL = "https://api.scp.smartsys.io/core/app-token";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Api-Key", ApiKey);
            client.DefaultRequestHeaders.Add("Api-Secret", ApiSecret);
            var response = client.GetAsync(URL);
            try
            {
                response.Wait();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task in GetAsync from SmartSys Token");
                return "Failed it Task Error J1 Error code 01" ;
            }
            if (!response.Result.IsSuccessStatusCode)
            {
                EventLog.WriteEntry("APIEngine", "Failed in response from Smartsys Server getting Token " + response.Result.ToString());
                return "Failed Server Request " + response.Result.StatusCode;
            }
            var r = client.GetStringAsync(URL);
            try
            {
                r.Wait();
            }catch(Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Failed Task in GetStringAsync from SmartSys Token");
                return "Failed it Task Error J1 Error code 02 ";
            }
            var z = r.Result;
            ApiReturnValues apiReturnValues= JsonConvert.DeserializeObject<ApiReturnValues>(z);
            if (apiReturnValues.status == "ok")
            {
                return apiReturnValues.token;
            }
            EventLog.WriteEntry("APIEngine", "The Token from Smartsys was not ok ");
            return "Failed";
        }
        public class ApiReturnValues
        {
            public string status { get; set; }
            public string token { get; set; }
        }
}
    public class ExpirableToken
    {
        public string token { get; set; }
        public DateTime expiryTime { get; set; }
    }
}