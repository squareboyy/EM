using EM.Data;
using EM.MenuItems;
using EM.Windows;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
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
    public partial class MainWindow : Window
    {
        private DBConnection conn;
        private DBDataAccess action = new();
        private string myConnectionString = "server = localhost; uid=root; password = EMDataBase; database = EM";
        private readonly MenuSelectTable selectTable = new();
        private readonly MenuFile menuFile = new();
        private string table = "pollution";

        public MainWindow()
        {
            InitializeComponent();
            conn = new DBConnection(myConnectionString);
        }

        private void PollutionClick(object sender, RoutedEventArgs e)
        {
            table = "pollution";
            UpdateDataGrid();
            ActionMenuItems();
        }

        private void PollutantClick(object sender, RoutedEventArgs e)
        {
            table = "pollutant";
            UpdateDataGrid();
            ActionMenuItems();
            //CarcinogenicRiskButton.IsChecked = false;
        }

        private void ObjectClick(object sender, RoutedEventArgs e)
        {
            table = "object";
            UpdateDataGrid();
            ActionMenuItems();
            //selectTable.GetTableObject(dataGrid, conn);
        }

        public void UpdateDataGrid()
        {
            switch (table)
            {
                case "object":
                    //dataGrid.Items.Refresh();
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

        private void ActionMenuItems()
        {
            switch (table)
            {
                case "object":
                    objectTableItem.Foreground = Brushes.Black;
                    pollutionTableItem.Foreground = Brushes.SlateGray;
                    pollutanTableItem.Foreground = Brushes.SlateGray;

                    CarcinogenicRiskButton.Visibility = Visibility.Hidden;
                    NonCarcinogenicRiskButton.Visibility = Visibility.Hidden;
                    CompensationButton.Visibility = Visibility.Hidden;
                    TaxesButton.Visibility = Visibility.Hidden;
                    EmergencySituationsButton.Visibility = Visibility.Hidden;

                    break;

                case "pollutant":
                    pollutanTableItem.Foreground = Brushes.Black;
                    pollutionTableItem.Foreground = Brushes.SlateGray;
                    objectTableItem.Foreground = Brushes.SlateGray;

                    CarcinogenicRiskButton.Visibility = Visibility.Hidden;
                    NonCarcinogenicRiskButton.Visibility = Visibility.Hidden;
                    CompensationButton.Visibility = Visibility.Hidden;
                    TaxesButton.Visibility = Visibility.Hidden;
                    EmergencySituationsButton.Visibility = Visibility.Hidden;

                    break;

                case "pollution":
                    pollutionTableItem.Foreground = Brushes.Black;
                    pollutanTableItem.Foreground = Brushes.SlateGray;
                    objectTableItem.Foreground = Brushes.SlateGray;

                    CarcinogenicRiskButton.Visibility = Visibility.Visible;
                    NonCarcinogenicRiskButton.Visibility = Visibility.Visible;
                    CompensationButton.Visibility = Visibility.Visible;
                    TaxesButton.Visibility = Visibility.Visible;
                    EmergencySituationsButton.Visibility = Visibility.Visible;

                    AddDataButton.IsEnabled = true;
                    CarcinogenicRiskButton.IsEnabled = true;
                    NonCarcinogenicRiskButton.IsEnabled = true;
                    CompensationButton.IsEnabled = true;
                    TaxesButton.IsEnabled = true;
                    EmergencySituationsButton.IsEnabled = true;

                    break;
            }
        }

        private void AddData(object sender, RoutedEventArgs e)
        {
            switch (table)
            {
                case "object":
                    AddObject(sender, e);
                    break;

                case "pollutant":
                    AddPollutant(sender, e);
                    break;

                case "pollution":
                    AddPollution(sender, e);
                    break;
            }
        }

        private void AddObject(object sender, RoutedEventArgs e)
        {
            string insertQuery = "INSERT INTO object (object_id, object_name, activity, ownership, population)" +
                                "VALUES (NULL, @value2, @value3, @value4, @value5)";
            menuFile.AddData(conn, insertQuery);
            ObjectClick(sender, e);
        }

        private void AddPollutant(object sender, RoutedEventArgs e)
        {
            string insertQuery = "INSERT INTO pollutant (pollutant_id, name_pollutant, gdk, avg_mass_consumption, danger_class, slope_factor, safe_exposure_level)" +
                                "VALUES (NULL, @value2, @value3, @value4, @value5, @value6, @value7)";
            menuFile.AddData(conn, insertQuery);
            PollutantClick(sender, e);
        }

        private void AddPollution(object sender, RoutedEventArgs e)
        {
            string insertQuery = "INSERT INTO pollution (pollution_id, object_id, pollutant_id, total_emissions, substance_concentration, pollution_date)" +
                                "VALUES (NULL, @value2, @value3, @value4, @value5, @value6)";
            menuFile.AddData(conn, insertQuery);

            action.StoredProcedure(conn, "CarcinogenicRisk");
            action.StoredProcedure(conn, "NonCarcinogenicRisk");
            action.StoredProcedure(conn, "Compensation");
            action.StoredProcedure(conn, "CalculationTaxAmount");
            action.StoredProcedure(conn, "DamagesEmergencySituations");

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

        private void CarcinogenicRisk(object sender, RoutedEventArgs e)
        {
            AddDataButton.IsEnabled = false;
            NonCarcinogenicRiskButton.IsEnabled = false;
            CompensationButton.IsEnabled = false;
            TaxesButton.IsEnabled = false;
            EmergencySituationsButton.IsEnabled = false;

            selectTable.GetCarcinogenicRisk(dataGrid, conn);
        }

        private void NonCarcinogenicRisk(object sender, RoutedEventArgs e)
        {
            AddDataButton.IsEnabled = false;
            CarcinogenicRiskButton.IsEnabled = false;
            CompensationButton.IsEnabled = false;
            TaxesButton.IsEnabled = false;
            EmergencySituationsButton.IsEnabled = false;

            selectTable.GetNonCarcinogenicRisk(dataGrid, conn);
        }

        private void CompensationCalculate(object sender, RoutedEventArgs e)
        {
            AddDataButton.IsEnabled = false;
            CarcinogenicRiskButton.IsEnabled = false;
            NonCarcinogenicRiskButton.IsEnabled = false;
            TaxesButton.IsEnabled = false;
            EmergencySituationsButton.IsEnabled = false;

            selectTable.GetCompensation(dataGrid, conn);
        }

        private void TaxesCalculate(object sender, RoutedEventArgs e)
        {
            AddDataButton.IsEnabled = false;
            CarcinogenicRiskButton.IsEnabled = false;
            NonCarcinogenicRiskButton.IsEnabled = false;
            CompensationButton.IsEnabled = false;
            EmergencySituationsButton.IsEnabled = false;

            selectTable.GetTaxes(dataGrid, conn);
        }

        private void EmergencySituationsCalculate(object sender, RoutedEventArgs e)
        {
            AddDataButton.IsEnabled = false;
            CarcinogenicRiskButton.IsEnabled = false;
            NonCarcinogenicRiskButton.IsEnabled = false;
            CompensationButton.IsEnabled = false;
            TaxesButton.IsEnabled = false;

            selectTable.GetEsDamages(dataGrid, conn);
        }

        private void DelSelectRow()
        {
            dataGrid.SelectedItem = null;
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            DelSelectRow();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
