using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace ODACFailoverConnectionSample
{
    public static class OracleDBManager
    {

        private static OracleConnection connection = null;

        private static string DB_HOST = "localhost"; // Host1 Address
        private static string DB_HOST2 = "localhost";// Host2 Address
        private static string DB_NAME = "orcl";
        private static string DB_USER = "system";
        private static string DB_PW = "123";

        public static string HostName
        {
            get
            {
                DBConnect();

                if (connection != null && connection.State == ConnectionState.Open)
                {
                    return connection.HostName;
                }
                else
                {
                    return null;
                }
            }
        }

        private static string GetConnectionString()
        {
            string connStr = string.Empty;

            connStr += $"Data Source=(DESCRIPTION=(CONNECT_TIMEOUT = 3)";
            connStr += $" (TRANSPORT_CONNECT_TIMEOUT = 3)";
            connStr += $" (LOAD_BALANCE = ON)";
            connStr += $" (FAILOVER = ON)";
            connStr += $" (ADDRESS_LIST=";
            connStr += $" (ADDRESS=(PROTOCOL=TCP)(HOST={DB_HOST})(PORT=1521))";
            connStr += $" (ADDRESS=(PROTOCOL=TCP)(HOST={DB_HOST2})(PORT=1521)))";
            connStr += $" (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={DB_NAME})";
            connStr += $" (FAILOVER_MODE = (TYPE = SESSION) (METHOD=BASIC) (RETRIES=3) )));";
            connStr += $" User ID={DB_USER};Password={DB_PW};";
            connStr += $" Connection Timeout=3;";

            return connStr;
        }


        private static void DBConnect()
        {
            if (connection == null)
            {
                connection = new OracleConnection(GetConnectionString());
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

        }

        public static DataTable GetDataTable(string strQuery)
        {
            try
            {
                DBConnect();

                DataTable dtTemp = new DataTable();
                using (OracleCommand command = new OracleCommand(strQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = strQuery;
                    using (OracleDataAdapter ObjDataAdapter = new OracleDataAdapter(command))
                    {
                        ObjDataAdapter.Fill(dtTemp);
                    }
                }

                return dtTemp;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"==OracleDBManager - GetDataTable=======================");
                Debug.WriteLine($" Exception        : {ex.Message}");
                Debug.WriteLine($" InnerException   : {ex.InnerException}");
                Debug.WriteLine($"=======================================================");
                return null;
            }

        }
    }
}
