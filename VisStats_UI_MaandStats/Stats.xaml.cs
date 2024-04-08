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
using VisStatsBL.model;

namespace VisStatsUI_MaandStats
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Stats : Window
    {
        public Stats(int jaar, int maand, List<MaandVangst> vangst, Haven haven)
        {
            InitializeComponent();
            HavenTextBox.Text = haven.ToString();
            JaarTextBox.Text = jaar.ToString();
            MaandTextBox.Text = maand.ToString();
            StatistiekenDataGrid.ItemsSource = vangst;
        }
    }
}
