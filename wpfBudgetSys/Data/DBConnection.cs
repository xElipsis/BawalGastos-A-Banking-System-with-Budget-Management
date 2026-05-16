using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace wpfBudgetSys.Database
{
    public class DBConnection
    {
        private static string connString = "server=localhost;user=root;database=banking_system;password=";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connString);
        }
    }
}
