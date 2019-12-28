using APIEngine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace APIEngine.Processes
{

    public class ProcessRequest
    {
        private string _j1var;
        Accumulator _accumulator;
        public ProcessRequest(string j1var)
        {
            _j1var = j1var;
        }
        public int Process()
        {
            var parms = GetParamList();
            if (parms == null)
            {
                EventLog.WriteEntry("APIEngine", "Error - No Entry for " + _j1var);
                return -1;
            }
            GetTotalValue(parms);
            while (!_accumulator.IsCompleted())
            {
                Thread.Sleep(200);
            }
            return _accumulator.GetTotal();
        }
        private List<Params> GetParamList()
        {
            MySqlCommunicator con = new MySqlCommunicator();
            var connectionOk = con.OpenConnection();
            List<Params> listOfSiteParams;
            if (connectionOk)
            {
                try
                {
                    listOfSiteParams = con.GetSitesParamsListSingleCall(_j1var);
                    return listOfSiteParams;
                }catch(Exception ex)
                {
                    EventLog.WriteEntry("APIEngine", "Database Error in getting Params from single call for " + _j1var);
                    return null;
                }
 
            }
            return null;
        }
        private void GetTotalValue(List<Params> p)
        {
            _accumulator = new Accumulator(p.Count);
            List<Params> listOfSiteParams = p;
            foreach (var site in listOfSiteParams)
            {
                Task.Factory.StartNew(() =>
                {
                    _accumulator.Addthis(getValue(site));
                });
            }
        }
        private int getValue(Params parms)
        {
            int value = 0;
            switch (parms.Api)
            {
                case "Smartsys":
                    SmartSys smartsys = new SmartSys(parms.param2, parms.param1);
                    var r = smartsys.GetApiResponse();
                    if (r.serverResponse == "ok")
                    {
                        value = r.vacant;
                    }
                    else
                    {
                        value = -1;
                    }
                    break;
                case "mooven":
                    Mooven mooven = new Mooven(parms.param1, parms.param3);
                    var m = mooven.GetTravelTime();
                    if (!Int32.TryParse(m, out value))
                    {
                        value = -1;
                    }
                    break;

                case "parkam":
                    Parkam parkam = new Parkam(parms.param1, parms.param2);
                    var p = parkam.GetCurrentVacant();
                    if (!Int32.TryParse(p, out value))
                    {
                        value = -1;
                    }

                    break;
                default:
                    break;
            }

            return value;
        }
    }

}