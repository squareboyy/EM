using EM.Data;
using EM.MenuItems;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    public partial class UpdateObjectWindow : Window
    {
        public UpdateObjectWindow(MenuUpdateRow main)
        {
            InitializeComponent();
            this.main = main;
            textBoxes = new[]
            {
                object_id,
                object_name,
                object_activity,
                object_ownership
            };
        }

        private MenuUpdateRow main;
        public TextBox[] textBoxes;

        private void UpdateData(object sender, RoutedEventArgs e)
        {
            string update = "UPDATE object SET object_name = @value2, activity = @value3, ownership = @value4 " +
                          "WHERE object_id = @value1";
            main.UpdateData(textBoxes, update);
            Close();
        }
    }
}
