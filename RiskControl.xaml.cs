using EM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EM
{
    public partial class RiskControl : UserControl
    {
        private DBConnection conn;
        private DBDataAccess action = new();
        private DataGrid dataGrid;

        public RiskControl(DBConnection conn, DataGrid dataGrid)
        {
            InitializeComponent();

            this.conn = conn;
            this.dataGrid = dataGrid;
        }
        private void CarcinogenicRisk(object sender, RoutedEventArgs e)
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

            dataGrid.Columns[1].Width = 400;
            dataGrid.Columns[2].Width = 300;
        }

        private void NonCarcinogenicRisk(object sender, RoutedEventArgs e)
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

            dataGrid.Columns[1].Width = 400;
            dataGrid.Columns[2].Width = 280;
        }
    }
}
