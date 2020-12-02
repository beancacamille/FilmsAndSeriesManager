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
        Methods methods = new Methods();

        public Film()
        {
            InitializeComponent();
            ComboStatus.ItemsSource = methods.RetrieveAllShowStatus();
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
            int type = 0;
            int status = ComboStatus.SelectedIndex;
            methods.AddFilm(title, url, score, type, status);
            ((MainWindow)this.Owner).UpdateLists();
            ((MainWindow)this.Owner).IsEnabled = true;
            Hide();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)this.Owner).IsEnabled = true;
            Hide();
        }
    }
}
