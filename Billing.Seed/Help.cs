using Billing.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Seed
{
    public static class Help
    {
        public static BillingContext Context = new BillingContext();
        public static string SourceRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SeedData\Billing.xls");

        public static Dictionary<int, int> dicAgen = new Dictionary<int, int>();
        public static Dictionary<int, int> dicProd = new Dictionary<int, int>();
        public static Dictionary<int, int> dicCatt = new Dictionary<int, int>();
        public static Dictionary<int, int> dicShip = new Dictionary<int, int>();
        public static Dictionary<int, int> dicSupp = new Dictionary<int, int>();
        public static Dictionary<int, int> dicCust = new Dictionary<int, int>();

        public static DataTable OpenExcel(string sheet)
        {
            var cs = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0", SourceRoot);
            OleDbConnection conn = new OleDbConnection(cs);
            conn.Open();

            OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}$]", sheet), conn);
            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();

            Console.Write($"{sheet}: ");
        
            return dt;
        }

        public static string getString(DataRow row, int index)
        {
            return row.ItemArray.GetValue(index).ToString();
        }

        public static int getInteger(DataRow row, int index)
        {
            return Convert.ToInt32(row.ItemArray.GetValue(index).ToString());
        }

        public static DateTime getDate(DataRow row, int index)
        {
            return Convert.ToDateTime(row.ItemArray.GetValue(index).ToString());
        }

        public static double getDouble(DataRow row, int index)
        {
            return Convert.ToDouble(row.ItemArray.GetValue(index).ToString());
        }
    }
}
