using EM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace EM.MenuItems
{
    public class MenuDeleteRow
    {
        public MenuDeleteRow(MainWindow main, DBConnection conn, string deleteQuery) 
        {
            this.main = main;
            this.conn = conn;
            this.deleteQuery = deleteQuery;
        }
      
        private MainWindow main;
        private DBConnection conn;
        private DBDataAccess action = new();
        private string deleteQuery;

        public void DeleteRow()
        {   
            try
            {
                conn.connect.Open();

                DataRowView rowView = (DataRowView)main.dataGrid.SelectedItem;
                string? ID = rowView[0].ToString();
                action.Delete(conn, deleteQuery, ID);
                main.UpdateDataGrid();

                conn.connect.Close();
            }
            catch
            {
                MessageBox.Show($"Помилка: оберіть рядок для видалення");
            } 
        }
    }
}
