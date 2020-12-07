using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FilmsAndSeriesManagerBusiness;
using System.Threading;
using System.Text.RegularExpressions;

namespace FilmsAndSeriesManagerWPF
{
    /// <summary>
    /// Interaction logic for Film.xaml
    /// </summary>
    public partial class Film : Window
    {
        Methods filmMethods;
        List<int> genreList;

        public Film(Methods methods)
        {
            InitializeComponent();
            filmMethods = methods;
            genreList = new List<int>();

            ComboStatus.ItemsSource = methods.RetrieveAllShowStatus();
            if (!filmMethods.IsSeries)
            {
                HideSeriesDetails();
            }
            if (filmMethods.IsShowEdit)
            {
                EditMode();
            }
        }

        private void HideSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Hidden;
            TxtSeason.Visibility = Visibility.Hidden;
            LblEpisode.Visibility = Visibility.Hidden;
            TxtEpisode.Visibility = Visibility.Hidden;
        }

        private void EditMode()
        {
            BtnAdd.Content = "Save";
            ComboStatus.SelectedIndex = filmMethods.SelectedShow.Status;
            TxtTitle.Text = filmMethods.SelectedShow.Title;
            TxtUrl.Text = filmMethods.SelectedShow.Url;
            SliderScore.Value = (double)filmMethods.SelectedShow.Score;
            TxtNotes.Text = filmMethods.SelectedShow.Notes;

            var elementList = ShowGrid.Children;
            foreach (var element in elementList)
            {
                var elementObject = element as object;
                if (elementObject is CheckBox)
                {
                    var checkBox = elementObject as CheckBox;
                    if (filmMethods.SelectedShow.GetAllGenreInt().Contains(int.Parse(checkBox.Tag.ToString())))
                    {
                        checkBox.IsChecked = true;
                    }
                }
            }
        }

        private void ShowErrorMessage(string message)
        {
            LblError.Visibility = Visibility.Visible;
            LblErrorValue.Visibility = Visibility.Visible;
            LblErrorValue.Content = message;
        }

        private void SliderScore_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblScoreValue.Content = (int)SliderScore.Value;
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            genreList.Add(int.Parse((sender as CheckBox).Tag.ToString()));
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            genreList.Remove(int.Parse((sender as CheckBox).Tag.ToString()));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = TxtTitle.Text.Trim();
            string url = TxtUrl.Text.Trim();
            int score = int.Parse(LblScoreValue.Content.ToString());
            int status = ComboStatus.SelectedIndex;
            string notes = TxtNotes.Text.Trim();
            string season = TxtSeason.Text.Trim();
            string episode = TxtEpisode.Text.Trim();

            //validations
            if (title == "")
            {
                ShowErrorMessage("Title is required");
            }
            else if (season == "" || Regex.IsMatch(season, @"[^0-9]+"))
            {
                ShowErrorMessage("Invalid season input");
            }
            else if (episode == "" || Regex.IsMatch(episode, @"[^0-9]+"))
            {
                ShowErrorMessage("Invalid episode input");
            }
            else
            {
                if (filmMethods.IsShowEdit)
                {
                    filmMethods.UpdateFilm(title, url, score, status, notes);
                    filmMethods.UpdateShowGenre(genreList);
                }
                else
                {
                    if (filmMethods.IsSeries)
                    {
                        filmMethods.AddSeries(title, url, score, 1, status, int.Parse(season), int.Parse(episode), notes);
                    }
                    else
                    {
                        filmMethods.AddFilm(title, url, score, 0, status, notes);
                    }
                    var selectedShow = filmMethods.GetShowByTitle(title);
                    genreList.Sort();
                    selectedShow.AddGenres(genreList);
                }
                CloseWindow();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            filmMethods.IsSeries = false;
            filmMethods.IsShowEdit = false;

            var mainWindow = new MainWindow(filmMethods);
            Hide();
            mainWindow.Show();
            Close();
        }
    }
}
