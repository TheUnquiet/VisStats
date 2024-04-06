using Microsoft.Win32;
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
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_DataUpload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string conn = @"Data Source=LAPTOP-RQN2J66V\SQLEXPRESS;Initial Catalog=VisStats;Integrated Security=True;Trust Server Certificate=True";
        IVisStatsRepository visStatsRepository;
        IFileProcessor fileProcessor = new FileProcessor();
        VisStatsManager visStatsManager;
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text documents (.txt)|*.txt";
            openFileDialog.DefaultDirectory = @"C:\data\vis";
            openFileDialog.Multiselect = true;
            visStatsRepository = new VisStatsRepository(conn);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);
        }

        private void Button_Click_Vissoorten(object sender, RoutedEventArgs e)
        {
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var filenames = openFileDialog.FileNames;
                VissoortenFileListbox.ItemsSource = filenames;
                openFileDialog.FileName = null;
            }
            else VissoortenFileListbox.ItemsSource = null;
        }
        private void Button_Click_Havens(object sender, RoutedEventArgs e)
        {
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var filenames = openFileDialog.FileNames;
                HavensFileListbox.ItemsSource = filenames;
                openFileDialog.FileName = null;
            }
            else HavensFileListbox.ItemsSource = null;
        }

        private void Button_Click_UploadVissoorten(object sender, RoutedEventArgs e)
        {
            foreach (string fileName in  VissoortenFileListbox.ItemsSource)
            {
                visStatsManager.UploadVis(fileName);
            }
            MessageBox.Show("Upload is klaar", "VisStats");
        }

        private void Button_Click_UploadHavens(object sender, RoutedEventArgs e)
        {
            foreach (string fileName in HavensFileListbox.ItemsSource)
            {
                visStatsManager.UploadHaven(fileName);
            }
            MessageBox.Show("Upload is klaar", "VistStats");
        }

        private void Button_Click_Statistieken(object sender, RoutedEventArgs e)
        {
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var filenames = openFileDialog.FileNames;
                StatistiekenFileListBox.ItemsSource = filenames;
                openFileDialog.FileName = null;
            } else StatistiekenFileListBox.ItemsSource= null;
        }

        private void Button_Click_UploadStatistieken(object sender, RoutedEventArgs e)
        {
            foreach(string filename in StatistiekenFileListBox.ItemsSource)
            {
                visStatsManager.UploadStatistieken(filename);
            }
            MessageBox.Show("Upload klaar", "VisStats");
        }
    }
}