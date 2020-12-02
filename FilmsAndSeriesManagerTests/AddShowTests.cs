using NUnit.Framework;
using FilmsAndSeriesManagerModel;
using FilmsAndSeriesManagerBusiness;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FilmsAndSeriesManagerTests
{
    public class AddFilmTests
    {
        Methods methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Your Name");
                foreach (var film in selectedFilm)
                {
                    db.Shows.Remove(film);
                }
                db.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Your Name");
                foreach (var film in selectedFilm)
                {
                    db.Shows.Remove(film);
                }
                db.SaveChanges();
            }
        }

        [Test]
        public void IfNewFilmIsAdded_ShowsCountIncreasesBy1()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                int count = db.Shows.ToList().Count();
                methods.AddFilm("Your Name", "", 0, 0, 0);

                Assert.AreEqual(count + 1, db.Shows.ToList().Count());
            }
        }
    }

    public class AddSeriesTests
    {
        Methods methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Detective Conan");
                if (selectedFilm.FirstOrDefault() != null)
                {
                    var selectedSeries = db.Series.Where(s => s.ShowId == selectedFilm.FirstOrDefault().Id);
                    foreach (var series in selectedSeries)
                    {
                        db.Series.Remove(series);
                    }
                }
                foreach (var film in selectedFilm)
                {
                    db.Shows.Remove(film);
                }
                db.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Detective Conan");
                if (selectedFilm.FirstOrDefault() != null)
                {
                    var selectedSeries = db.Series.Where(s => s.ShowId == selectedFilm.FirstOrDefault().Id);
                    foreach (var series in selectedSeries)
                    {
                        db.Series.Remove(series);
                    }
                }
                foreach (var film in selectedFilm)
                {
                    db.Shows.Remove(film);
                }
                db.SaveChanges();
            }
        }

        [Test]
        public void IfNewSeriesIsAdded_ShowsAndSeriesCountIncreaseBy1()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                int showsCount = db.Shows.ToList().Count();
                int seriesCount = db.Series.ToList().Count();
                methods.AddSeries("Detective Conan", "", 0, 1, 0, 0, 1);

                Assert.AreEqual(showsCount + 1, db.Shows.ToList().Count());
                Assert.AreEqual(seriesCount + 1, db.Series.ToList().Count());
            }
        }
    }
}