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
        Methods mainWindowMethods;

        public MainWindow()
        {
            InitializeComponent();
            mainWindowMethods = new Methods();
            mainWindowMethods.RetrieveAllShows();
            RadioTitle.IsChecked = true;
            ListWatching.SelectedIndex = 0;
        }

        public MainWindow(Methods methods)
        {
            InitializeComponent();
            mainWindowMethods = methods;
            mainWindowMethods.RetrieveAllShows();
            RadioTitle.IsChecked = true;
        }

        public void UpdateLists()
        {
            mainWindowMethods.PopulateShowCategoryLists();
            ListWatching.ItemsSource = mainWindowMethods.Watching;
            ListPlanToWatch.ItemsSource = mainWindowMethods.PlanToWatch;
            ListFinished.ItemsSource = mainWindowMethods.Finished;
            ListDropped.ItemsSource = mainWindowMethods.Dropped;
        }

        public void DisplayShowDetails()
        {
            LblTitleValue.Content = mainWindowMethods.SelectedShow.Title;
            LblUrlValue.Content = mainWindowMethods.SelectedShow.Url;
            LblTypeValue.Content = (mainWindowMethods.SelectedShow.Type == 0) ? "Film" : "Series";
            if (mainWindowMethods.SelectedShow.Type == 0)
            {
                HideSeriesDetails();
            }
            else
            {
                ShowSeriesDetails();
                LblSeasonValue.Content = mainWindowMethods.SelectedShow.Series.Season;
                LblEpisodeValue.Content = mainWindowMethods.SelectedShow.Series.Episode;
            }
            LblScoreValue.Content = mainWindowMethods.SelectedShow.Score;
            LblStatusValue.Content = mainWindowMethods.SelectedShow.StatusNavigation.Name;
            LblNotesValue.Content = mainWindowMethods.SelectedShow.Notes;
        }

        private void BtnAddFilm_Click(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.IsSeries = false;
            OpenFilmWindow();
        }

        private void BtnAddSeries_Click(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.IsSeries = true;
            OpenFilmWindow();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var radioName = (sender as RadioButton).Name;
            if (radioName == "RadioTitle")
            {
                mainWindowMethods.SortByTitle();
            }
            else
            {
                mainWindowMethods.SortByScore();
            }
            UpdateLists();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindowMethods.SelectedShow = (Show)(sender as ListBox).SelectedItem;
            DisplayShowDetails();
        }

        public void ShowSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Visible;
            LblSeasonValue.Visibility = Visibility.Visible;
            LblEpisode.Visibility = Visibility.Visible;
            LblEpisodeValue.Visibility = Visibility.Visible;
            BtnEdit.Visibility = Visibility.Visible;
        }

        public void HideSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Hidden;
            LblSeasonValue.Visibility = Visibility.Hidden;
            LblEpisode.Visibility = Visibility.Hidden;
            LblEpisodeValue.Visibility = Visibility.Hidden;
            BtnEdit.Visibility = Visibility.Hidden;
        }

        public void OpenFilmWindow()
        {
            var filmWindow = new Film(mainWindowMethods);
            Hide();
            filmWindow.Show();
            Close();
        }

        private void BtnEditShow_Click(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.IsShowEdit = true;
            OpenFilmWindow();
        }
    }
}
