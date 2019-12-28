using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SmartSysParkingObject
    {
        public List<Groups> groups { get; set; }
    }
    public class Group
    {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string organization { get; set; }
        public string parent { get; set; }
        public string type { get; set; }
    }

    public class Metadata
    {
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string location { get; set; }
        public string timeZone { get; set; }
        public string v2siteCode { get; set; }
        public string BayCount { get; set; }
        public string BayType { get; set; }
        public string City { get; set; }
        public string HasSensor { get; set; }
        public string IsUnMarked { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LotCode { get; set; }
        public string MaxStayPeriod { get; set; }
        public string OperatingHourCode { get; set; }
        public string Street { get; set; }
        public string SubZone { get; set; }
        public string TariffCode { get; set; }
        public string Ward { get; set; }
        public string Zone { get; set; }
    }

    public class Groups
    {
        public Group group { get; set; }
        public Metadata metadata { get; set; }
    }
}
