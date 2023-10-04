using EM.Data;
using EM.MenuItems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
using System.Windows.Shapes;

namespace EM
{
    public partial class UpdatePollutionWindow : Window
    {
        public UpdatePollutionWindow(MenuUpdateRow main, DBConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
            this.main = main;

            textBoxes = new[]
            {
                pollution_id,
                object_id,
                name_pollutant,
                total_emissions,
                pollution_date
            };

            objectView = action.SelectTable(this.conn, "SELECT object_name FROM object").DefaultView;
            ComboObjectClass.ItemsSource = objectView;
            ComboObjectClass.DisplayMemberPath = "object_name";

            pollutantView = action.SelectTable(this.conn, "SELECT name_pollutant FROM pollutant").DefaultView;
            ComboPollutantClass.ItemsSource = pollutantView;
            ComboPollutantClass.DisplayMemberPath = "name_pollutant";
        }

        private DBConnection conn;
        private DataView objectView;
        private DataView pollutantView;
        public TextBox[] textBoxes;
        private MenuUpdateRow main;
        private DBDataAccess action = new();

        private void UpdateData(object sender, RoutedEventArgs e)
        {
            string updateQuery = "UPDATE pollution SET object_id = @value2, pollutant_id = @value3, " +
                            "total_emissions = @value4, pollution_date = @value5 " +
                            "WHERE pollution_id = @value1";

            main.UpdateData(textBoxes, updateQuery);
            Close();
        }

        private void ComboObjectClassChanged(object sender, SelectionChangedEventArgs e)
        {
            conn.connect.Open();

            string selectIdQuery = "SELECT object_id FROM object WHERE object_name = @value1";
            DataRowView rowView = (DataRowView)ComboObjectClass.SelectedItem;
            textBoxes[1].Text = action.Select(conn, selectIdQuery, rowView[0].ToString());

            conn.connect.Close();
        }

        private void ComboPollutantClassChanged(object sender, SelectionChangedEventArgs e)
        {
            conn.connect.Open();

            string selectIdQuery = "SELECT pollutant_id FROM pollutant WHERE name_pollutant = @value1";
            DataRowView rowView = (DataRowView)ComboPollutantClass.SelectedItem;
            textBoxes[2].Text = action.Select(conn, selectIdQuery, rowView[0].ToString());

            conn.connect.Close();
        }

        private void PollutionWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataRowView? selectedRow = null;
            foreach (DataRowView row in objectView)
            {
                if (row["object_name"].ToString() == textBoxes[1].Text)
                {
                    selectedRow = row;
                    break;
                }
            }
            ComboObjectClass.SelectedItem = selectedRow;

            foreach (DataRowView row in pollutantView)
            {
                if (row["name_pollutant"].ToString() == textBoxes[2].Text)
                {
                    selectedRow = row;
                    break;
                }
            }
            ComboPollutantClass.SelectedItem = selectedRow;
        }
    }
}
