using System;
using System.Data;
using System.Threading;

namespace ODACFailoverConnectionSample
{
    class Program
    {

        static void Main(string[] args)
        {
            Timer timer = new Timer(TimerCallback, null, 0, 1000);
            Console.ReadLine();
        }

        private static void TimerCallback(object state)
        {
            Console.WriteLine($"DB Host : {OracleDBManager.HostName}, DBCurrentTime : {GetDBCurrentTime()}");
        }

        public static string GetDBCurrentTime()
        {
            string query = " SELECT sysdate FROM dual ";
            DataTable dt = OracleDBManager.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }

            return null;
        }
    }

}
