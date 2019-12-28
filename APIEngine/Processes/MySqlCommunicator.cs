
using APIEngine.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIEngine.Processes
{
    public class MySqlCommunicator
    {
       // string connectionString = "server=localhost;database=j1apidb;uid=root;pwd='Seahorse99';";
       // string connectionString = "server=softgineering.com;database=j1api;uid=ianbennetts;pwd='SeahorseBronte9!';";
        string connectionString = "server=103.18.40.138;database=hosts;uid=root;pwd='pvms044';";
        MySqlConnection connection;
        public bool OpenConnection()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    return true;
                }
                EventLog.WriteEntry("APIEngine", "Database connected but did not open");
                return false;
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Database Error *****"+ex.Message);
                CloseConnection();
                return false;
            }
        }
        public List<Params> GetSitesParamsListSingleCall(string id)
        {
            List<Params> paramList = new List<Params>();
            Params parms;
            var cmd = new MySqlCommand();
            var command = "select j1apikey.*, j1apivalues.* from j1apikey ";
            command += "inner join j1apimap on (j1apimap.api_key_id=j1apikey.idJ1APIKEY) ";
            command += "inner join j1apivalues on (j1apimap.api_value_id=j1apivalues.ValueId) ";
            command += "where j1apikey.J1Ref=@param1;";
            cmd.CommandText = command;
            cmd.Parameters.AddWithValue("@param1", id);
            cmd.Connection = connection;
            MySqlDataReader reader = null; ;
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    parms = new Params();
                    parms.param1 = reader.GetString("Parm1");
                    parms.param2 = reader.GetString("Parm2");
                    parms.param3 = reader.GetString("Parm3");
                    parms.Api = reader.GetString("API");
                    paramList.Add(parms);
                }
                reader.Close();
                CloseConnection();
                return paramList;
            }catch(Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Database Failed to read GetSitesParamSingleCall ");
                if (reader != null) reader.Close();
                CloseConnection();
                return null;
            }
        
        }
        private void CloseConnection()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("APIEngine", "Unable to Close Connection");
            }
        }

    }
}