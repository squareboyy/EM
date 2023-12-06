using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using EM.Data;

namespace EM.MenuItems
{
    internal class MenuSelectTable
    {
        private readonly DBDataAccess action = new();

        public void GetTableObject(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT*FROM object";

            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Назва об'єкта";
            dataGrid.Columns[2].Header = "Вид діяльності";
            dataGrid.Columns[3].Header = "Форма власності";
            dataGrid.Columns[4].Header = "Популяція";
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
            dataGrid.Columns[5].Header = "Фактор нахилу (мг/кс*доба)^(-1)";
            dataGrid.Columns[6].Header = "Безпечний рівень впливу (мг/м^3)";
        }

        public void GetTablePollution(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT pollution_id, object_name, name_pollutant, total_emissions, substance_concentration, " +
                            "pollution_date " +
                            "FROM pollution " +
                            "JOIN object USING(object_id) " +
                            "JOIN pollutant USING(pollutant_id) " +
                            "ORDER BY pollution_id";

            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Об'єкт";
            dataGrid.Columns[2].Header = "Речовина";
            dataGrid.Columns[3].Header = "Усього викидів (т/рік)";
            dataGrid.Columns[4].Header = "Концентрація речовини (мг/м^3)";
            dataGrid.Columns[5].Header = "Звітний рік";

            dataGrid.Columns[1].Width = 220;
            dataGrid.Columns[4].Width = 130;
        }

        public void GetCarcinogenicRisk(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT pollution_id, object_name, name_pollutant, total_emissions, substance_concentration, " +
                           "carcinogenic_risk, characteristic, pollution_date " +
                           "FROM pollution " +
                           "JOIN object USING(object_id) " +
                           "JOIN pollutant USING(pollutant_id) " +
                           "JOIN carcinogenic_risk_assessment USING(pollution_id) " +
                           "ORDER BY pollution_id";

            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Об'єкт";
            dataGrid.Columns[2].Header = "Речовина";
            dataGrid.Columns[3].Header = "Усього викидів (т/рік)";
            dataGrid.Columns[4].Header = "Концентрація речовини (мг/м^3)";
            dataGrid.Columns[5].Header = "Канцерогенний ризик";
            dataGrid.Columns[6].Header = "Рівень ризику";
            dataGrid.Columns[7].Header = "Звітний рік";

            dataGrid.Columns[1].Width = 220;
            dataGrid.Columns[4].Width = 130;
        }

        public void GetNonCarcinogenicRisk(DataGrid dataGrid, DBConnection conn)
        {
            string select = "SELECT pollution_id, object_name, name_pollutant, total_emissions, substance_concentration, " +
                           "noncarcinogenic_risk, characteristic, pollution_date " +
                           "FROM pollution " +
                           "JOIN object USING(object_id) " +
                           "JOIN pollutant USING(pollutant_id) " +
                           "JOIN noncarcinogenic_risk_assessment USING(pollution_id) " +
                           "ORDER BY pollution_id";

            dataGrid.ItemsSource = action.SelectTable(conn, select).DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Об'єкт";
            dataGrid.Columns[2].Header = "Речовина";
            dataGrid.Columns[3].Header = "Усього викидів (т/рік)";
            dataGrid.Columns[4].Header = "Концентрація речовини (мг/м^3)";
            dataGrid.Columns[5].Header = "Неканцерогенний ризик";
            dataGrid.Columns[6].Header = "Рівень ризику";
            dataGrid.Columns[7].Header = "Звітний рік";

            dataGrid.Columns[1].Width = 220;
            dataGrid.Columns[4].Width = 100;
        }

        public void GetCompensation(DataGrid dataGrid, DBConnection conn)
        {
            dataGrid.ItemsSource = action.SelectTable(conn, "SELECT * FROM compensation_pollution").DefaultView;

            dataGrid.Columns[0].Header = "ID";
            dataGrid.Columns[1].Header = "Об'єкт";
            dataGrid.Columns[2].Header = "Популяція";
            dataGrid.Columns[3].Header = "Речовина";
            dataGrid.Columns[4].Header = "Усього викидів(т / рік)";
            dataGrid.Columns[5].Header = "Масова витрата (т/рік)";
            dataGrid.Columns[6].Header = "ГДК (мг/м^3)";
            dataGrid.Columns[7].Header = "Звітний рік";
            dataGrid.Columns[8].Header = "Компенсація";

            dataGrid.Columns[1].Width = 220;
            dataGrid.Columns[3].Width = 200;
        }
    }
}
