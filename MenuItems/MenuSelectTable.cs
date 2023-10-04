using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EM.Data;

namespace EM.MenuItems
{
    internal class MenuSelectTable
    {
        private DBDataAccess action = new();
        public void GetTableObject(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT*FROM object";
            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Назва об'єкта";
            dataGrid.Columns[2].Header = "Вид діяльності";
            dataGrid.Columns[3].Header = "Форма власності";
        }

        public void GetTablePollutant(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT*FROM pollutant";
            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Назва речовини";
            dataGrid.Columns[2].Header = "ГДК (мг/м^3)";
            dataGrid.Columns[3].Header = "Масова витрата (т/рік)";
            dataGrid.Columns[4].Header = "Клас небезпеки";
        }

        public void GetTablePollution(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT pollution_id, object_name, name_pollutant, total_emissions, pollution_date FROM pollution" +
                            " JOIN object USING(object_id) JOIN pollutant USING(pollutant_id) ORDER BY pollution_id";
            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Об'єкт";
            dataGrid.Columns[2].Header = "Речовина";
            dataGrid.Columns[3].Header = "Усього викидів (т/рік)";
            dataGrid.Columns[4].Header = "Звітний рік";
        }
    }
}
