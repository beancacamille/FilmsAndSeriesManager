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

        public Film(Methods methods)
        {
            InitializeComponent();
            filmMethods = methods;
            ComboStatus.ItemsSource = methods.RetrieveAllShowStatus();
            if (!filmMethods.IsSeries)
            {
                HideSeriesDetails();
            }
        }

        private void SliderScore_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblScoreValue.Content = (int)SliderScore.Value;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = TxtTitle.Text;
            string url = TxtUrl.Text;
            int score = int.Parse(LblScoreValue.Content.ToString());
            int status = ComboStatus.SelectedIndex;
            string notes = TxtNotes.Text;

            if (filmMethods.IsSeries)
            {
                int season = int.Parse(TxtSeason.Text);
                int episode = int.Parse(TxtEpisode.Text);
                filmMethods.AddSeries(title, url, score, 1, status, season, episode, notes);
                filmMethods.IsSeries = false;
            }
            else
            {
                filmMethods.AddFilm(title, url, score, 0, status, notes);
            }
            CloseWindow();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            filmMethods.IsSeries = false;
            CloseWindow();
        }

        private void HideSeriesDetails()
        {
            LblSeason.Visibility = Visibility.Hidden;
            TxtSeason.Visibility = Visibility.Hidden;
            LblEpisode.Visibility = Visibility.Hidden;
            TxtEpisode.Visibility = Visibility.Hidden;
        }

        private void CloseWindow()
        {
            var mainWindow = new MainWindow(filmMethods);
            Hide();
            mainWindow.Show();
            Close();
        }
    }
}
