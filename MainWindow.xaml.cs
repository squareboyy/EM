using EM.Data;
using EM.MenuItems;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EM
{
    public partial class MainWindow : Window
    {
        private DBConnection conn;
        private string myConnectionString = "server = localhost; uid=root; password = EMDataBase; database = EM";
        public MainWindow()
        {
            InitializeComponent();
            conn = new DBConnection(myConnectionString);
        }

        private readonly MenuSelectTable selectTable = new ();
        private readonly MenuFile menuFile = new ();
        private string table = "";

        private void ObjectClick(object sender, RoutedEventArgs e)
        {
            DelSelectRow();
            table = "object";
            UpdateDataGrid();
        }

        private void PollutantClick(object sender, RoutedEventArgs e)
        {
            DelSelectRow();
            table = "pollutant";
            UpdateDataGrid();
        }

        private void PollutionClick(object sender, RoutedEventArgs e)
        {
            DelSelectRow();
            table = "pollution";
            UpdateDataGrid();
        }

        public void UpdateDataGrid()
        {
            switch (table)
            {
                case "object":
                    selectTable.GetTableObject(dataGrid, conn);
                    break;

                case "pollutant":
                    selectTable.GetTablePollutant(dataGrid, conn);
                    break;

                case "pollution":
                    selectTable.GetTablePollution(dataGrid, conn);
                    break;
            }
        }

        private void AddObject(object sender, RoutedEventArgs e)
        {
            string insertQuery = "INSERT INTO object (object_id, object_name, activity, ownership)" +
                                "VALUES (NULL, @value2, @value3, @value4)";
            menuFile.AddData(conn, insertQuery);
            ObjectClick(sender, e);
        }

        private void AddPollutant(object sender, RoutedEventArgs e)
        {
            string insertQuery = "INSERT INTO pollutant (pollutant_id, name_pollutant, gdk, avg_mass_consumption, danger_class)" +
                                "VALUES (NULL, @value2, @value3, @value4, @value5)";
            menuFile.AddData(conn, insertQuery);
            PollutantClick(sender, e);
        }

        private void AddPollution(object sender, RoutedEventArgs e)
        {
            string insertQuery = "INSERT INTO pollution (pollution_id, object_id, pollutant_id, total_emissions, pollution_date)" +
                                "VALUES (NULL, @value2, @value3, @value4, @value5)";
            menuFile.AddData(conn, insertQuery);
            PollutionClick(sender, e);
        }

        private void UpdateRow(object sender, RoutedEventArgs e)
        {
            MenuUpdateRow menuUpdateRow = new(this, conn);

            switch (table)
            {
                case "object":
                    UpdateObjectWindow window_object = new(menuUpdateRow);
                    menuUpdateRow.UpdateRow(window_object, window_object.textBoxes);
                    break;

                case "pollutant":
                    UpdatePollutantWindow window_pollutant = new(menuUpdateRow);
                    menuUpdateRow.UpdateRow(window_pollutant, window_pollutant.textBoxes);
                    break;

                case "pollution":
                    UpdatePollutionWindow window_pollution = new(menuUpdateRow, conn);
                    menuUpdateRow.UpdateRow(window_pollution, window_pollution.textBoxes);
                    break;
            }

            DelSelectRow();
        }

        private void DeleteRow(object sender, RoutedEventArgs e)
        {
            MenuDeleteRow menuDeleteRow;

            switch (table)
            {
                case "object":
                    menuDeleteRow = new(this, conn, "DELETE FROM object WHERE object_id = @value1");
                    menuDeleteRow.DeleteRow();
                    break;

                case "pollutant":
                    menuDeleteRow = new(this, conn, "DELETE FROM pollutant WHERE pollutant_id = @value1");
                    menuDeleteRow.DeleteRow();
                    break;

                case "pollution":
                    menuDeleteRow = new(this, conn, "DELETE FROM pollution WHERE pollution_id = @value1");
                    menuDeleteRow.DeleteRow();
                    break;
            }

            DelSelectRow();
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateItem.IsEnabled = true;
            deleteItem.IsEnabled = true;
        }

        private void DelSelectRow()
        {
            dataGrid.SelectedItem = null;
            updateItem.IsEnabled = false;
            deleteItem.IsEnabled = false;
        }

        private void MainWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            DelSelectRow();
        }

        private void MyWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
