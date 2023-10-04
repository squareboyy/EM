using EM.MenuItems;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace EM
{
    public partial class UpdatePollutantWindow : Window
    {
        public UpdatePollutantWindow(MenuUpdateRow main)
        {
            InitializeComponent();
            this.main = main;
            textBoxes = new[]
            {
                pollutant_id,
                name_pollutant,
                gdk,
                avg_mass_consumption,
                danger_class
            };
            ComboDangerClass.ItemsSource = dangerClases;
        }

        private List<string> dangerClases = new() { "I", "II", "III", "IV", " " };
        public TextBox[] textBoxes;
        private MenuUpdateRow main;

        private void UpdateData(object sender, RoutedEventArgs e)
        {
            string update = "UPDATE pollutant SET name_pollutant = @value2, gdk = @value3, " +
                            "avg_mass_consumption = @value4, danger_class = @value5 " +
                            "WHERE pollutant_id = @value1";
            main.UpdateData(textBoxes, update);
            Close();
        }
        private void ComboDangerClassChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxes[4].Text = ComboDangerClass.SelectedItem.ToString();
        }

        private void PollutantWindowLoaded(object sender, RoutedEventArgs e)
        {
            ComboDangerClass.SelectedItem = textBoxes[4].Text;
        }
    }
}
