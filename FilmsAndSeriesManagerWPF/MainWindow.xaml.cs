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
        Film filmWindow = new Film();

        public MainWindow()
        {
            InitializeComponent();
            methods.RetrieveAllShows();
            RadioTitle.IsChecked = true;
        }

        public void UpdateLists()
        {
            methods.PopulateShowCategoryLists();
            ListWatching.ItemsSource = methods.Watching;
            ListPlanToWatch.ItemsSource = methods.PlanToWatch;
            ListFinished.ItemsSource = methods.Finished;
            ListDropped.ItemsSource = methods.Dropped;
        }

        public void DisplayShowDetails()
        {
            LblTitleValue.Content = methods.SelectedShow.Title;
            LblUrlValue.Content = methods.SelectedShow.Url;
            LblTypeValue.Content = (methods.SelectedShow.Type == 0) ? "Film" : "Series";
            LblScoreValue.Content = methods.SelectedShow.Score;
            LblStatusValue.Content = methods.SelectedShow.Status;
            LblNotesValue.Content = methods.SelectedShow.Notes;
        }

        private void BtnAddFilm_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            filmWindow.Show();
            Close();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var radioName = (sender as RadioButton).Name;
            if (radioName == "RadioTitle")
            {
                methods.SortByTitle();
            }
            else
            {
                methods.SortByScore();
            }
            UpdateLists();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            methods.SelectedShow = (Show)(sender as ListBox).SelectedItem;
            DisplayShowDetails();
        }
    }
}
