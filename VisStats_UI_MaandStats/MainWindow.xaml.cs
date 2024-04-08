using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisStatsBL.interfaces;
using VisStatsBL.managers;
using VisStatsBL.model;
using VisStatsDL_File;
using VisStatsDL_SQL;
using VisStatsUI_MaandStats;

namespace VisStats_UI_MaandStats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string conn = @"Data Source=LAPTOP-RQN2J66V\SQLEXPRESS;Initial Catalog=VisStats;Integrated Security=True;TrustServerCertificate=True";
        IFileProcessor fileProcessor;
        IVisStatsRepository visStatsRepository;
        VisStatsManager visStatsManager;

        ObservableCollection<MaandVangst> maandVangsten = new ObservableCollection<MaandVangst>();
        ObservableCollection<Haven> Havens;

        public MainWindow()
        {
            InitializeComponent();
            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(conn);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);

            SoortComboBox.ItemsSource = visStatsManager.GeefVissoorten();
            
            JaarComboBox.ItemsSource = visStatsManager.GeefJaartallen();

            MaandComboBox.ItemsSource = Enumerable.Range(1, 12);

        }

        private void ToonStatsButton_Click(object sender, RoutedEventArgs e)
        {
            /*List<MaandVangst> maandVangst = visStatsManager.GeefMaandVangst(
                (int)JaarComboBox.SelectedItem, (int)MaandComboBox.SelectedItem, (Vissoort)SoortComboBox.SelectedItem, 
                (Haven) HavenComboBox.SelectedItem);
            foreach (MaandVangst vangst in  maandVangst)
            {
                maandVangsten.Add(vangst);
            }

            Stats stats = new Stats((int)JaarComboBox.SelectedItem, (int)MaandComboBox.SelectedItem, maandVangst, (Haven)HavenComboBox.SelectedItem);
            stats.ShowDialog();
            */
        }
    }
}