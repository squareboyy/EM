using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace EM.Data
{
    public class DBConnection
    {
        public MySqlConnection connect { get; }

        public DBConnection(string connectionString)
        {
            connect = new MySqlConnection(connectionString);
            ConnectionDB();
        }

        public void ConnectionDB()
        {
            try
            {
                connect.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connect.Close();
            }
        }
    }
}
