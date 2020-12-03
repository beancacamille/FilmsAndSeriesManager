using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilmsAndSeriesManagerModel;
using Microsoft.EntityFrameworkCore;

namespace FilmsAndSeriesManagerBusiness
{
    public class Methods
    {
        public List<Show> ShowList { get; set; }
        public List<Show> Watching { get; set; }
        public List<Show> PlanToWatch { get; set; }
        public List<Show> Finished { get; set; }
        public List<Show> Dropped { get; set; }
        public List<Genre> GenreList { get; set; }
        public Show SelectedShow { get; set; }

        public void RetrieveAllShows()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                ShowList =  db.Shows.Include(s => s.Series).ToList();
            }
        }

        public void PopulateShowCategoryLists()
        {
            Watching = ShowList.Where(s => s.Status == 0).ToList();
            PlanToWatch = ShowList.Where(s => s.Status == 1).ToList();
            Finished = ShowList.Where(s => s.Status == 2).ToList();
            Dropped = ShowList.Where(s => s.Status == 3).ToList();
        }

        public List<ShowStatus> RetrieveAllShowStatus()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                return db.ShowStatuses.ToList();
            }
        }

        public List<ShowGenre> RetrieveAllGenre()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                return db.ShowGenres.ToList();
            }
        }

        public Show GetShowByTitle(string title)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                return db.Shows.Where(s => s.Title == title).FirstOrDefault();
            }
        }

        public void AddFilm(string title, string url, int score, int type, int status)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = title.Trim(), Url = url.Trim(), Score = score, Type = type, Status = status};
                db.Shows.Add(newFilm);
                db.SaveChanges();
            }
        }

        public void AddSeries(string title, string url, int score, int type, int status, int season, int episode)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = title.Trim(), Url = url.Trim(), Score = score, Type = type, Status = status};
                db.Shows.Add(newFilm);
                db.SaveChanges();

                var selectedFilm = db.Shows.Where(f => f.Title == title).FirstOrDefault();
                var newSeries = new Series { Show = selectedFilm, Season = season, Episode = episode };
                db.Series.Add(newSeries);

                db.SaveChanges();
            }
        }

        public void UpdateFavourite()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedShow = db.Shows.Where(s => s.Id == SelectedShow.Id).FirstOrDefault();
                selectedShow.Favourite = !selectedShow.Favourite;
                db.SaveChanges();
            }
        }

        public void UpdateFilm(string title, string url, int score, int status, string notes)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedShow = db.Shows.Where(s => s.Id == SelectedShow.Id).FirstOrDefault();
                selectedShow.Title = title;
                selectedShow.Url = url;
                selectedShow.Score = score;
                selectedShow.Status = status;
                selectedShow.Notes = notes;
                db.SaveChanges();
            }
        }

        public void DeleteShow()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedShow = db.Shows.Where(s => s.Id == SelectedShow.Id).FirstOrDefault();
                if (selectedShow.Type == 1)
                {
                    var selectedSeries = db.Series.Where(s => s.ShowId == selectedShow.Id).FirstOrDefault();
                    db.Series.Remove(selectedSeries);
                }
                var showGenres = db.ShowGenres.Where(s => s.ShowId == selectedShow.Id);
                foreach (var item in showGenres)
                {
                    db.ShowGenres.Remove(item);
                }
                db.Shows.Remove(selectedShow);
                db.SaveChanges();
            }
        }

        public void SortByTitle()
        {
            ShowList = ShowList.OrderBy(s => s.Title).ThenByDescending(s => s.Score).ToList();
        }

        public void SortByScore()
        {
            ShowList = ShowList.OrderByDescending(s => s.Score).ToList();
        }
    }
}
