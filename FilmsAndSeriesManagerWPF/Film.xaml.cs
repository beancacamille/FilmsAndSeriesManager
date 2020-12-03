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
            string title = TxtTitle.Text;
            string url = TxtUrl.Text;
            int score = int.Parse(LblScoreValue.Content.ToString());
            int status = ComboStatus.SelectedIndex;
            string notes = TxtNotes.Text;

            if (filmMethods.IsShowEdit)
            {
                filmMethods.UpdateFilm(title, url, score, status, notes);
            }
            else
            {
                if (filmMethods.IsSeries)
                {
                    int season = int.Parse(TxtSeason.Text);
                    int episode = int.Parse(TxtEpisode.Text);
                    filmMethods.AddSeries(title, url, score, 1, status, season, episode, notes);
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
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
