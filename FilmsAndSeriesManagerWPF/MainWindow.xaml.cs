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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FilmsAndSeriesManagerBusiness;
using FilmsAndSeriesManagerModel;

namespace FilmsAndSeriesManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Methods methods = new Methods();

        public MainWindow()
        {
            InitializeComponent();
            methods.PopulateShowLists();
            ListWatching.ItemsSource = methods.Watching;
            ListPlanToWatch.ItemsSource = methods.PlanToWatch;
            ListFinished.ItemsSource = methods.Finished;
            ListDropped.ItemsSource = methods.Dropped;
        }

        private void BtnAddFilm_Click(object sender, RoutedEventArgs e)
        {
            var filmWindow = new Film();
            filmWindow.Show();
        }
    }
}
