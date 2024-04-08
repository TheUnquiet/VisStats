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

namespace VisStatsUI_Stats
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

        ObservableCollection<Vissoort> AlleVissoorten;
        ObservableCollection<Vissoort> GeselecteerdeVissoorten;

        public MainWindow()
        {
            InitializeComponent();
            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(conn);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);

            HavensComboBox.ItemsSource = visStatsManager.GeefHavens();
            HavensComboBox.SelectedIndex = 0;
            JaarComboBox.ItemsSource = visStatsManager.GeefJaartallen();
            JaarComboBox.SelectedIndex = 0;

            AlleVissoorten = new ObservableCollection<Vissoort>(visStatsManager.GeefVissoorten());
            AlleSoortenListBox.ItemsSource = AlleVissoorten;
            GeselecteerdeVissoorten = new ObservableCollection<Vissoort>();      GeselecteerdeSoortenListBox.ItemsSource=GeselecteerdeVissoorten;
        }

        private void VoegAlleSoortenToeButton_CLick(object sender, RoutedEventArgs e)
        {
            foreach (Vissoort v in AlleVissoorten)
            {
                GeselecteerdeVissoorten.Add(v);
            }
            AlleVissoorten.Clear();
        }

        private void VoegSoortenToeButton_CLick(object sender, RoutedEventArgs e)
        {
            List<Vissoort> soorten = new();
            foreach (Vissoort v in AlleSoortenListBox.SelectedItems) soorten.Add(v);
            foreach (Vissoort v in soorten)
            {
                GeselecteerdeVissoorten.Add(v);
                AlleVissoorten.Remove(v);
            }
        }

        private void VerwijderSoortenButton_CLick(object sender, RoutedEventArgs e)
        {
            List<Vissoort> soorten = new();
            foreach (Vissoort v in GeselecteerdeSoortenListBox.SelectedItems) soorten.Add(v);
            foreach (Vissoort v in soorten)
            {
                GeselecteerdeVissoorten.Remove(v);
                AlleVissoorten.Add(v);
            }
        }

        private void VerwijderAlleSoortenButton_CLick(object sender, RoutedEventArgs e)
        {
            foreach (Vissoort v in GeselecteerdeVissoorten)
            {
                AlleVissoorten.Add(v);
            }
            GeselecteerdeVissoorten.Clear();
        }

        private void ToonStatistiekenButton_CLick(object sender, RoutedEventArgs e)
        {
            Eenheid eenheid;
            if ((bool)kgRadioButton.IsChecked) eenheid = Eenheid.kg; else eenheid = Eenheid.euro;
            List<JaarVangst> vangsts = visStatsManager.GeefVangst((int)JaarComboBox.SelectedItem, (Haven) HavensComboBox.SelectedItem, GeselecteerdeVissoorten.ToList(), eenheid);
            Statistieken s = new Statistieken((int)JaarComboBox.SelectedItem, (Haven) HavensComboBox.SelectedItem, vangsts, eenheid);
            s.ShowDialog();
        }
    }
}