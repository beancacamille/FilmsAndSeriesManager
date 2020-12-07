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
        private int _selectedStatus = 0;
        private int _selectedIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            mainWindowMethods = new Methods();
            mainWindowMethods.RetrieveAllShows();
            RadioTitle.IsChecked = true;
            SelectFirstItem();
            mainWindowMethods.GenreFilter = new List<int>();
        }

        public MainWindow(Methods methods)
        {
            InitializeComponent();
            mainWindowMethods = methods;
            mainWindowMethods.RetrieveAllShows();
            RadioTitle.IsChecked = true;
            SelectFirstItem();
            mainWindowMethods.GenreFilter = new List<int>();
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
            if (mainWindowMethods.SelectedShow != null)
            {
                ShowImageGrid.Visibility = Visibility.Hidden;
                ShowDetailsGrid.Visibility = Visibility.Visible;

                BtnFavourite.Visibility = Visibility.Visible;
                BtnFavourite.Content = (mainWindowMethods.SelectedShow.Favourite) ? "Remove from Favourites" : "Add to Favourites";
                LblTitleValue.Content = mainWindowMethods.SelectedShow.Title;
                LblUrlValue.Content = (mainWindowMethods.SelectedShow.Url.Length > 35) ? mainWindowMethods.SelectedShow.Url.Substring(0, 35) + "..." : mainWindowMethods.SelectedShow.Url;
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
                TxtBlockGenre.Text = mainWindowMethods.SelectedShow.GetAllGenreString();
                TxtBlockNotes.Text = mainWindowMethods.SelectedShow.Notes;

                BtnEditShow.Visibility = Visibility.Visible;
                BtnDeleteShow.Visibility = Visibility.Visible;
            }
            else
            {
                ShowDetailsGrid.Visibility = Visibility.Hidden;
                ShowImageGrid.Visibility = Visibility.Visible;
            }
        }

        private void ShowSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Visible;
            LblSeasonValue.Visibility = Visibility.Visible;
            if (LblSeasonValue.Content.ToString() == "0")
            {
                LblSeason.Content = "Episode";
                LblSeasonValue.Content = LblEpisodeValue.Content;
                LblEpisode.Visibility = Visibility.Hidden;
                LblEpisodeValue.Visibility = Visibility.Hidden;
            }
            else
            {
                LblSeason.Content = "Season";
                LblEpisode.Visibility = Visibility.Visible;
                LblEpisodeValue.Visibility = Visibility.Visible;
            }
            BtnEdit.Content = "Edit";
            BtnEdit.Visibility = Visibility.Visible;

            TxtSeason.Visibility = Visibility.Hidden;
            TxtEpisode.Visibility = Visibility.Hidden;
            BtnMinus.Visibility = Visibility.Hidden;
            BtnPlus.Visibility = Visibility.Hidden;
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
            LblSeason.Content = "Season";
            LblSeason.Visibility = Visibility.Visible;
            LblSeasonValue.Visibility = Visibility.Hidden;
            LblEpisode.Visibility = Visibility.Visible;
            LblEpisodeValue.Visibility = Visibility.Hidden;
            TxtSeason.Visibility = Visibility.Visible;
            TxtEpisode.Visibility = Visibility.Visible;
            BtnMinus.Visibility = Visibility.Visible;
            BtnPlus.Visibility = Visibility.Visible;
            BtnEdit.Content = "Save";
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
            mainWindowMethods.FilterFavourites(CheckFavourites.IsChecked == true);
            mainWindowMethods.SearchByTitle(TxtSearch.Text);
            mainWindowMethods.FilterByGenre();
            SortList();
            UpdateLists();
        }

        private void SaveSelectedStatusAndIndex()
        {
            _selectedStatus = mainWindowMethods.SelectedShow.Status;
            switch (_selectedStatus)
            {
                case 0:
                    _selectedIndex = ListWatching.SelectedIndex;
                    break;
                case 1:
                    _selectedIndex = ListPlanToWatch.SelectedIndex;
                    break;
                case 2:
                    _selectedIndex = ListFinished.SelectedIndex;
                    break;
                case 3:
                    _selectedIndex = ListDropped.SelectedIndex;
                    break;
            }
        }

        private void SelectEditedItem()
        {
            switch (_selectedStatus)
            {
                case 0:
                    ListWatching.SelectedIndex = _selectedIndex;
                    break;
                case 1:
                    ListPlanToWatch.SelectedIndex = _selectedIndex;
                    break;
                case 2:
                    ListFinished.SelectedIndex = _selectedIndex;
                    break;
                case 3:
                    ListDropped.SelectedIndex = _selectedIndex;
                    break;
            }
        }

        private void SelectFirstItem()
        {
            if (ListWatching.Items.Count > 0) ListWatching.SelectedIndex = 0;
            else if (ListPlanToWatch.Items.Count > 0) ListPlanToWatch.SelectedIndex = 0;
            else if (ListFinished.Items.Count > 0) ListFinished.SelectedIndex = 0;
            else if (ListDropped.Items.Count > 0) ListDropped.SelectedIndex = 0;
            else DisplayShowDetails();
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

        private void ShowFavourites(object sender, RoutedEventArgs e)
        {
            SearchFilterSort();
            SelectFirstItem();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchFilterSort();
            SelectFirstItem();
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
            SelectFirstItem();
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            mainWindowMethods.GenreFilter.Remove(int.Parse((sender as CheckBox).Tag.ToString()));
            SearchFilterSort();
            SelectFirstItem();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TxtSearch.Text = "";
            RadioTitle.IsChecked = true;

            var elementList = FilterGrid.Children;
            foreach (var element in elementList)
            {
                var elementObject = element as object;
                if (elementObject is CheckBox)
                {
                    var checkBox = elementObject as CheckBox;
                    if (checkBox.Tag != null)
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindowMethods.SelectedShow = (Show)(sender as ListBox).SelectedItem;
            DisplayShowDetails();
        }

        private void BtnFavourite_Click(object sender, RoutedEventArgs e)
        {
            SaveSelectedStatusAndIndex();
            mainWindowMethods.UpdateFavourite();
            SearchFilterSort();
            if (CheckFavourites.IsChecked == true)
            {
                SelectFirstItem();
            }
            else
            {
                SelectEditedItem();
            }
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
                SaveSelectedStatusAndIndex();
                ShowEditSeriesElements();
                TxtSeason.Text = mainWindowMethods.SelectedShow.Series.Season.ToString();
                TxtEpisode.Text = mainWindowMethods.SelectedShow.Series.Episode.ToString();
            }
            else
            {
                int season = int.Parse(TxtSeason.Text);
                int episode = int.Parse(TxtEpisode.Text);
                mainWindowMethods.UpdateSeriesDetails(season, episode);
                ShowSeriesDetails();
                SearchFilterSort();
                SelectEditedItem();
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
            SelectFirstItem();
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
