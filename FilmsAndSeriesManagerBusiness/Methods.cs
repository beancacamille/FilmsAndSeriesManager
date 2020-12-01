using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilmsAndSeriesManagerModel;

namespace FilmsAndSeriesManagerBusiness
{
    public class Methods
    {
        public void AddFilm(string title, string url, int type, int status)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = title.Trim(), Url = url.Trim(), Type = type, Status = status };
                db.Shows.Add(newFilm);
                db.SaveChanges();
            }
        }

        public void AddSeries(string title, string url, int type, int status, int season, int episode)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = title.Trim(), Url = url.Trim(), Type = type, Status = status };
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
