using System;
using System.Collections.Generic;

namespace FilmsAndSeriesManagerBusiness
{
    class Program
    {
        static void Main(string[] args)
        {
            Methods methods = new Methods();

            //methods.AddFilm("Just test", "",1 , 0, 2, "");
            //Console.WriteLine("Film added");
            //var selectedShow = methods.GetShowByTitle("Just test");
            //selectedShow.AddGenres(new List<int> { 0, 1, 2, 3 });
            //Console.WriteLine("Genres added");

            //methods.AddFilm("Lalala", "", 0, 0, 0);
            //var selectedFilm = methods.GetShowByTitle("Lalala");
            //Console.WriteLine(selectedFilm.Favourite);
            //methods.SelectedShow = selectedFilm;
            //methods.UpdateFavourite();
            //selectedFilm = methods.GetShowByTitle("Lalala");
            //Console.WriteLine(selectedFilm.Favourite);

            //methods.AddFilm("Butler", "", 10, 0, 3);
            //var selectedShow = methods.GetShowByTitle("Butler");
            //methods.SelectedShow = selectedShow;
            //methods.UpdateFilm("Butler", "a.com", 9, 2, "This is a note.");
            //selectedShow = methods.GetShowByTitle("Butler");
            //Console.WriteLine(selectedShow.Url + " " + selectedShow.Notes);

            //methods.RetrieveAllShows();
            //int count = methods.ShowList.Count;
            //var selectedShow = methods.GetShowByTitle("Butler");
            //methods.SelectedShow = selectedShow;
            //methods.DeleteShow();
            //methods.RetrieveAllShows();
            //int count2 = methods.ShowList.Count;
            //Console.WriteLine(count + " " + count2);
        }
    }
}
