using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Data
{
    internal class DBDataAccess
    {
        public DataTable SelectTable(DBConnection conn, string selectQuery)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand(selectQuery, conn.connect);

            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
            {
                adapter.Fill(dataTable);
            }

            return dataTable;
        }

        public string? Select(DBConnection conn, string selectQuery, string? searchVariable)
        {
            MySqlCommand command = new MySqlCommand(selectQuery, conn.connect);
            command.Parameters.AddWithValue($"@value{1}", searchVariable);
            object answer = command.ExecuteScalar();

            if(answer != null)
            {
                return answer.ToString();
            }
            else
            {
                return "";
            }
        }

        public void InsertInto(DBConnection conn, List<object?> tableObjects, string insertQuery)
        {
            MySqlCommand command = new MySqlCommand(insertQuery, conn.connect);

            for (int i = 0; i < tableObjects.Count; i++)
            {
                command.Parameters.AddWithValue($"@value{i + 1}", tableObjects[i]);
            }

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void Update(DBConnection conn, List<object?> tableObjects, string updateQuery)
        {
            MySqlCommand command = new MySqlCommand(updateQuery, conn.connect);

            for (int i = 0; i < tableObjects.Count; i++)
            {
                command.Parameters.AddWithValue($"@value{i + 1}", tableObjects[i]);
            }

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void Delete(DBConnection conn, string deleteQuery, string? ID)
        {
            MySqlCommand command = new MySqlCommand(deleteQuery, conn.connect);
            command.Parameters.AddWithValue($"@value{1}", ID);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void AlterTable(DBConnection conn, string alterQuery)
        {
            MySqlCommand command = new MySqlCommand(alterQuery, conn.connect);

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void StoredProcedure(DBConnection conn, string nameProcedure)
        {
            conn.connect.Open();

            MySqlCommand command = new MySqlCommand(nameProcedure, conn.connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Prepare();
            command.ExecuteNonQuery();

            conn.connect.Close();
        }
    }
}
