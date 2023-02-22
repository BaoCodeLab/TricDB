using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.Extensions
{
    public class MySQLDB
    {
        /// <summary>
        /// 快速读取表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> GetSimpleTFromQuery<T>(string query) where T: new ()
        {
            string connectionString = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            MySqlConnection cn = new MySqlConnection(connectionString);
            MySqlDataAdapter cmd = new MySqlDataAdapter(query, cn);
            DataSet ds = new DataSet();
            cmd.Fill(ds);
            var list = SQLHelper.GetList<T>(ds.Tables[0], string.Empty, true);
            return list;
        }
        public static List<T> GetSimpleTFromQuery<T>(string query,MySqlParameter[] param) where T : new()
        {
            var ds = GetDataSet(query, param);
            var list = SQLHelper.GetList<T>(ds.Tables[0], string.Empty, true);
            return list;
        }
        public static DataTable GetSimpleDataFromQuery(string query)
        {
            string connectionString = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            MySqlConnection cn = new MySqlConnection(connectionString);
            MySqlDataAdapter cmd = new MySqlDataAdapter(query, cn);
            DataSet ds = new DataSet();
            cmd.Fill(ds);
            return ds.Tables[0];
        }
        public static string GetSimpleJsonFromQuery(string query)
        {
            string ret = "[";
            List<string> list = new List<string>();
            string connectionString = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            MySqlConnection cn = new MySqlConnection(connectionString);
            MySqlDataAdapter cmd = new MySqlDataAdapter(query, cn);
            DataSet ds = new DataSet();
            cmd.Fill(ds);
            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                List<string> current = new List<string>();
                for(int j=0;j<ds.Tables[0].Columns.Count;j++)
                {
                    string cr = "\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":\"" + ds.Tables[0].Rows[i][j].ToString() + "\"";
                    current.Add(cr);
                }
                list.Add(string.Join(",", current.ToArray()));
            }
            ret += string.Join(",\r\n", list.ToArray());
            ret += "]";
            return ret;
        }
        public static DataSet GetDataSet( string query, params MySqlParameter[] cmdParameters)
        {
            DataSet ds = new DataSet();
            string connectionString=AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = query,
                    CommandType = CommandType.Text
                };
                if (cmdParameters != null && cmdParameters.Count() > 0)
                {
                    foreach (var r in cmdParameters)
                    {
                        cmd.Parameters.Add(r);
                    }
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
            return ds;
        }
        public static int ExecuteNonQuery(string query, params MySqlParameter[] commandParameters)
        {
            MySqlCommand cmd = new MySqlCommand();
            string connectionString = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.CommandText = query;
                if(commandParameters!=null)
                {
                    foreach(var r in commandParameters)
                    {
                        cmd.Parameters.Add(r);
                    }
                }
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                connection.Close();
                return val;
            }
        }
    }
}
