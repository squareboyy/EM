using EM.Data;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EM.MenuItems
{
    internal class MenuFile
    {
        public void AddData(DBConnection conn, string insertQuery)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    conn.connect.Open();

                    string filePath = openFileDialog.FileName;
                    Action(conn, insertQuery, filePath);
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1452)
                    {
                        MessageBox.Show("Помилка, спочатку заповніть таблиці: object та pollutant");

                        DBDataAccess action = new();
                        action.AlterTable(conn, "ALTER TABLE pollution AUTO_INCREMENT = 1");
                    }
                    else
                    {
                        MessageBox.Show("Помилка, перевірте коректність даних в excel відповідно до таблиці, яку ви хочете заповнити");
                    }
                }
                finally
                {
                    conn.connect.Close();
                }
            }
        }

        private void Action(DBConnection conn, string insertQuery, string filePath)
        {
            DBDataAccess action = new();
            List<object?> list = new();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value.ToString() != "NULL" && !string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value.ToString()))
                        {
                            list.Add(worksheet.Cells[row, col].Value);
                        }
                        else
                        {
                            list.Add(DBNull.Value);
                        }
                    }

                    action.InsertInto(conn, list, insertQuery);
                    list.Clear();
                }
            }
        }
    }
}
