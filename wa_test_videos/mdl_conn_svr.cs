using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace wa_transcript
{
    public class mdl_conn_svr
    {
        public static string strconn_sql
        {

            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "localhost";
                builder.InitialCatalog = "tmp_javs";
                builder.UserID = "puntocero";
                builder.Password = "dev_.0";
                builder.ConnectTimeout = 0;

                //MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
                //conn_string.in = "";
                //conn_string.UserID = "";
                //conn_string.Password = "";
                //conn_string.Database = "";
                //conn_string.Port = 0;
                ////conn_string.ConnectionTimeout = 0;

                return builder.ConnectionString;

            }
        }
    }
}