
using APIEngine.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SqlConnection
    {
        string connectionString = "server=localhost;database=j1apidb;uid=root;pwd='Seahorse99';";
        MySqlConnection connection;
        public bool OpenConnection()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            if (connection.State==ConnectionState.Open)
            {
                return true;
            }
            return false;
        }
        public List<string> ReadKey(string param)
        {
            var cmd = new MySqlCommand();
            cmd.CommandText = "SELECT * FROM j1apidb.j1apikey where J1Ref = @param1 ";
            cmd.Parameters.AddWithValue("@param1", param);
            cmd.Connection = connection;
            MySqlDataReader reader = cmd.ExecuteReader();
            string columnValue = "";
            while (reader.Read())
            {
                columnValue = reader.GetString(3);
            }
            List<string> listOfSites = new List<string>();
            listOfSites.AddRange(columnValue.Split(' '));
            reader.Close();
            return listOfSites;
        }
        public List<Params> GetSitesParamsList(List<string> siteIds)
        {
            List<Params> paramList = new List<Params>();
            foreach (var site in siteIds)
            {
                paramList.Add(GetParams(site));
            }
            return paramList;
        }
        public Params GetParams(string id)
        {
            Params p=new Params();
            var cmd = new MySqlCommand();
            cmd.CommandText = "SELECT * FROM j1apidb.j1apivalues where ValueId= @param1 ";
            cmd.Parameters.AddWithValue("@param1", id);
            cmd.Connection = connection;
            MySqlDataReader reader = cmd.ExecuteReader();       
            while (reader.Read())
            {
                p.param1 = reader.GetString(1);
                p.param2 = reader.GetString(2);
                p.param3 = reader.GetString(3);
                p.Api = reader.GetString(4);
            }
            reader.Close();
            return p;
        }
            
    }
}
