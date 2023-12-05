using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using System.Text.RegularExpressions;
using System.Diagnostics;
using EM.Data;

namespace EM.MenuItems
{
    public class MenuUpdateRow
    {
        public MenuUpdateRow(MainWindow main, DBConnection conn)
        {
            this.main = main;
            this.conn = conn;
        }

        private MainWindow main;
        private DBConnection conn;
        private DBDataAccess action = new();

        public void UpdateRow(Window window, TextBox[] textBoxes)
        {
            if (main.dataGrid.SelectedItem != null)
            {
                try
                {
                    DataRowView rowView = (DataRowView)main.dataGrid.SelectedItem;

                    for (int i = 0; i < rowView.Row.ItemArray.Length; i++)
                    {
                        textBoxes[i].Text = rowView[i].ToString();
                    }

                    window.Show();
                }
                catch
                {
                    MessageBox.Show($"Помилка: цю таблицю не можно змінити");
                }
            }
            else
            {
                MessageBox.Show($"Помилка: оберіть рядок для оновлення");
            }
        }

        public void UpdateData(TextBox[] textBoxes, string update)
        {
            conn.connect.Open();
            List<object?> list = new();

            try
            {
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(textBoxes[i].Text))
                    {
                        if (double.TryParse(textBoxes[i].Text, out double value))
                        {
                            list.Add(value);
                        }
                        else
                        {
                            list.Add(textBoxes[i].Text);
                        }
                    }
                    else
                    {
                        list.Add(DBNull.Value);
                    }
                }

                action.Update(conn, list, update);
                main.UpdateDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
            finally
            {
                conn.connect.Close();
            }
        }
    }
}
