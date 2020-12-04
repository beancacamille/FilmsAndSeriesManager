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
using System.Diagnostics;

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
            mainWindowMethods.GenreFilter = new List<int>();
        }

        public MainWindow(Methods methods)
        {
            InitializeComponent();
            mainWindowMethods = methods;
            mainWindowMethods.RetrieveAllShows();
            RadioTitle.IsChecked = true;
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
            BtnFavourite.Content = (mainWindowMethods.SelectedShow.Favourite) ? "Yes" : "No";
            LblTitleValue.Content = mainWindowMethods.SelectedShow.Title;
            LblUrlValue.Content = mainWindowMethods.SelectedShow.Url;
            if (LblUrlValue.Content.ToString() == "")
            {
                BtnGo.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnGo.Visibility = Visibility.Visible;
            }
            LblTypeValue.Content = (mainWindowMethods.SelectedShow.Type == 0) ? "Film" : "Series";
            if (mainWindowMethods.SelectedShow.Type == 0)
            {
                HideSeriesDetails();
            }
            else
            {
                LblSeasonValue.Content = mainWindowMethods.SelectedShow.Series.Season;
                LblEpisodeValue.Content = mainWindowMethods.SelectedShow.Series.Episode;
                ShowSeriesDetails();
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
            SearchFilterSort();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            SortList();
            UpdateLists();
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.GenreFilter.Add(int.Parse((sender as CheckBox).Tag.ToString()));
            SearchFilterSort();
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.GenreFilter.Remove(int.Parse((sender as CheckBox).Tag.ToString()));
            SearchFilterSort();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindowMethods.SelectedShow = (Show)(sender as ListBox).SelectedItem;
            if (mainWindowMethods.SelectedShow != null)
            {
                DisplayShowDetails();
                HideEditSeriesElements();
            }
        }

        private void BtnFavourite_Click(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.UpdateFavourite();
            SearchFilterSort();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = mainWindowMethods.SelectedShow.Url;
            myProcess.Start();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var btnContent = (sender as Button).Content.ToString();
            if (btnContent == "Edit")
            {
                ShowEditSeriesElements();
                TxtSeason.Text = mainWindowMethods.SelectedShow.Series.Season.ToString();
                TxtEpisode.Text = mainWindowMethods.SelectedShow.Series.Episode.ToString();
            }
            else
            {
                int season = int.Parse(TxtSeason.Text);
                int episode = int.Parse(TxtEpisode.Text);
                mainWindowMethods.UpdateSeriesDetails(season, episode);
                HideEditSeriesElements();
                SearchFilterSort();
            }
        }

        private void BtnPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            var btnContent = (sender as Button).Content.ToString();
            if (btnContent == "+")
            {
                TxtEpisode.Text = (int.Parse(TxtEpisode.Text.Trim()) + 1).ToString();
            }
            else
            {
                if (TxtEpisode.Text != "0")
                {
                    TxtEpisode.Text = (int.Parse(TxtEpisode.Text.Trim()) - 1).ToString();
                }
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
            SearchFilterSort();
            
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

        private void SearchFilterSort()
        {
            mainWindowMethods.RetrieveAllShows();
            mainWindowMethods.SearchByTitle(TxtSearch.Text);
            mainWindowMethods.FilterByGenre();
            SortList();
            UpdateLists();
        }

        private void ShowSeriesDetails()
        {
            if (LblSeasonValue.Content.ToString() == "0")
            {
                LblSeason.Visibility = Visibility.Hidden;
                LblSeasonValue.Visibility = Visibility.Hidden;
            }
            else
            {
                LblSeason.Visibility = Visibility.Visible;
                LblSeasonValue.Visibility = Visibility.Visible;
            }
            LblEpisode.Visibility = Visibility.Visible;
            LblEpisodeValue.Visibility = Visibility.Visible;
            BtnEdit.Content = "Edit";
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

        private void ShowEditSeriesElements()
        {
            LblSeasonValue.Visibility = Visibility.Hidden;
            LblEpisodeValue.Visibility = Visibility.Hidden;
            TxtSeason.Visibility = Visibility.Visible;
            TxtEpisode.Visibility = Visibility.Visible;
            BtnMinus.Visibility = Visibility.Visible;
            BtnPlus.Visibility = Visibility.Visible;
            BtnEdit.Content = "Save";
        }

        private void HideEditSeriesElements()
        {
            TxtSeason.Visibility = Visibility.Hidden;
            TxtEpisode.Visibility = Visibility.Hidden;
            BtnMinus.Visibility = Visibility.Hidden;
            BtnPlus.Visibility = Visibility.Hidden;
            ShowSeriesDetails();
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
