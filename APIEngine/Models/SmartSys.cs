using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

namespace APIEngine.Models
{
    public class SmartSys
    {
        string _apiKey = "9jNwgogyARHAfKDwPIUO3icWo2ExyC1C";
        string _orgID;
        string _groupRef;
        int _retrys = 4;
        public SmartSys(string orgID, string groupRef)
        {
            _orgID = orgID;
            _groupRef = groupRef;
        }
        public SmartSysResponse GetApiResponse()
        {
            int counter = 0;
            Random random = new Random();
            SmartSysResponse response= GetApiResponse1();
            while (response.serverResponse!="ok" && counter++ < _retrys)
            {
                Thread.Sleep(random.Next(2, 10) * 1000);
                response = GetApiResponse1();
            }

            return response;
        }
        private SmartSysResponse GetApiResponse1()
        {
            var timer = new Stopwatch();
            timer.Start();
            string URL ="https://api.scp.smartsys.io/parking/organizations/mornington/groups/"+_orgID+"%23"+_groupRef+"/occupancy";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Api-Key", _apiKey);
            string token = SmartSysAuthToken.GetToken().token;
            if (token.Length < 100)
            {
                SmartSysResponse errorResponse = new SmartSysResponse();
                errorResponse.serverResponse = token;
                return errorResponse;
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = client.GetAsync(URL);
            try
            {
                response.Wait();
            }catch(Exception ex)
            {
                SmartSysResponse smartSysResponse = new SmartSysResponse();
                smartSysResponse.serverResponse = "Failed Task J1 Error No 3 " ;
                EventLog.WriteEntry("APIEngine", "Failed Task GetAsync Smartsys");
                return smartSysResponse;
            }
            if (!response.Result.IsSuccessStatusCode)
            {
                SmartSysResponse smartSysResponse = new SmartSysResponse();
                smartSysResponse.serverResponse= "Failed Server Request " + response.Result.ToString();
                EventLog.WriteEntry("APIEngine", "Failed Server Request " + response.Result.ToString());
                return smartSysResponse;
            }
            var r = client.GetStringAsync(URL);
            try
            {
               r.Wait();
            }
            catch (Exception ex)
            {
                SmartSysResponse smartSysResponse = new SmartSysResponse();
                smartSysResponse.serverResponse = "Failed Task J1 Error No 4 ";
                EventLog.WriteEntry("APIEngine", "Failed Task GetStringAsync Smartsys");
                return smartSysResponse;
            }
            var z = r.Result;
            SmartSysResponse apiReturnValues = JsonConvert.DeserializeObject<SmartSysResponse>(z);
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            apiReturnValues.serverResponseTime= timeTaken.ToString(@"m\:ss\.fff");
            apiReturnValues.serverResponse = "ok";
            return apiReturnValues;
        }
    }
    public class SmartSysResponse
    {
        public int occupied { get; set; }
        public int vacant { get; set; }
        public int unknown { get; set; }
        public int total { get; set; }
        public string serverResponse { get; set; }
        public string serverResponseTime { get; set; }
    } 
}