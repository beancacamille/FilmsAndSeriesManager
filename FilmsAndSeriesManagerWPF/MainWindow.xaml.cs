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
            //AutomaticallySelectItemOnList();
        }

        private void UpdateLists()
        {
            mainWindowMethods.PopulateShowCategoryLists();
            ListWatching.ItemsSource = mainWindowMethods.Watching;
            ListPlanToWatch.ItemsSource = mainWindowMethods.PlanToWatch;
            ListFinished.ItemsSource = mainWindowMethods.Finished;
            ListDropped.ItemsSource = mainWindowMethods.Dropped;
        }

        private void DisplayShowDetails()
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
            LblGenreValue.Content = mainWindowMethods.SelectedShow.GetAllGenreString();
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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindowMethods.RetrieveAllShows();
            mainWindowMethods.SearchByTitle(TxtSearch.Text);
            SortList();
            UpdateLists();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            SortList();
            UpdateLists();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindowMethods.SelectedShow = (Show)(sender as ListBox).SelectedItem;
            if (mainWindowMethods.SelectedShow != null)
            {
                DisplayShowDetails();
            }
        }

        private void BtnEditShow_Click(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.IsShowEdit = true;
            OpenFilmWindow();
        }

        private void BtnDeleteShow_Click(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.DeleteShow();
            mainWindowMethods.RetrieveAllShows();
            SortList();
            UpdateLists();
            
            if (ListWatching.Items.Count > 0)
            {
                ListWatching.SelectedIndex = 0;
            }
        }

        private void SortList()
        {
            if (RadioTitle.IsChecked == true)
            {
                mainWindowMethods.SortByTitle();
            }
            else
            {
                mainWindowMethods.SortByScore();
            }
        }

        private void ShowSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Visible;
            LblSeasonValue.Visibility = Visibility.Visible;
            LblEpisode.Visibility = Visibility.Visible;
            LblEpisodeValue.Visibility = Visibility.Visible;
            BtnEdit.Visibility = Visibility.Visible;
        }

        private void HideSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Hidden;
            LblSeasonValue.Visibility = Visibility.Hidden;
            LblEpisode.Visibility = Visibility.Hidden;
            LblEpisodeValue.Visibility = Visibility.Hidden;
            BtnEdit.Visibility = Visibility.Hidden;
        }

        private void OpenFilmWindow()
        {
            var filmWindow = new Film(mainWindowMethods);
            Hide();
            filmWindow.Show();
            Close();
        }
    }
}
