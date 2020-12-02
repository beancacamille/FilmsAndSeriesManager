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
        public List<Genre> GenreList { get; set; }
        public List<Show> Watching { get; set; }
        public List<Show> PlanToWatch { get; set; }
        public List<Show> Finished { get; set; }
        public List<Show> Dropped { get; set; }

        public void PopulateShowLists()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                Watching = db.Shows.Include(s => s.Series).Where(s => s.Status == 0).ToList();
                PlanToWatch = db.Shows.Include(s => s.Series).Where(s => s.Status == 1).ToList();
                Finished = db.Shows.Include(s => s.Series).Where(s => s.Status == 2).ToList();
                Dropped = db.Shows.Include(s => s.Series).Where(s => s.Status == 3).ToList();
            }
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
                var newFilm = new Show { Title = title.Trim(), Url = url.Trim(), Score = score, Type = type, Status = status };
                db.Shows.Add(newFilm);
                db.SaveChanges();
            }
        }

        public void AddSeries(string title, string url, int score, int type, int status, int season, int episode)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = title.Trim(), Url = url.Trim(), Score = score, Type = type, Status = status };
                db.Shows.Add(newFilm);
                db.SaveChanges();

                var selectedFilm = db.Shows.Where(f => f.Title == title).FirstOrDefault();
                var newSeries = new Series { Show = selectedFilm, Season = season, Episode = episode };
                db.Series.Add(newSeries);

                db.SaveChanges();
            }
        }
    }
}
