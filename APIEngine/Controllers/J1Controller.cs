using APIEngine.Processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIEngine.Controllers
{
    public class J1Controller : ApiController
    {
        // GET: api/J1
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/J1/5
        public string Get(string id)
        {
            if (!EventLog.SourceExists("APIEngine"))
                EventLog.CreateEventSource("APIEngine", "APIEngine Log");
            EventLog.WriteEntry("APIEngine", "Get Request for " + id);
            if (id == null || id.Length > 5)
            {
                EventLog.WriteEntry("APIEngine", "Id failed length test too long " + id);
                return "Failed";
            }
            ProcessRequest request = new ProcessRequest(id);
            var value = request.Process();
            EventLog.WriteEntry("APIEngine", "Get Response for " + id + " was " + value.ToString());
            return value.ToString();
        }

        // POST: api/J1
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/J1/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/J1/5
        public void Delete(int id)
        {
        }
    }
}
