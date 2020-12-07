using NUnit.Framework;
using FilmsAndSeriesManagerModel;
using FilmsAndSeriesManagerBusiness;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FilmsAndSeriesManagerTests
{
    class AddFilmTests
    {
        Methods _methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Test Show");
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
                var selectedFilm = db.Shows.Where(f => f.Title == "Test Show");
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
                _methods.AddFilm("Test Show", "", 0, 0, 0, "");

                Assert.AreEqual(count + 1, db.Shows.ToList().Count());
            }
        }

        [Test]
        public void IfNewFilmIsAdded_DetailsAreCorrect()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                _methods.AddFilm("Test Show", "example.com", 5, 0, 0, "Testing");

                var selectedFilm = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();

                Assert.AreEqual("example.com", selectedFilm.Url);
                Assert.AreEqual(5, selectedFilm.Score);
                Assert.AreEqual(0, selectedFilm.Type);
                Assert.AreEqual(0, selectedFilm.Status);
                Assert.AreEqual("Testing", selectedFilm.Notes);
            }
        }
    }

    class AddSeriesTests
    {
        Methods _methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Test Show");
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
                var selectedFilm = db.Shows.Where(f => f.Title == "Test Show");
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

                _methods.AddSeries("Test Show", "example.com", 0, 1, 0, 0, 1, "");

                Assert.AreEqual(showsCount + 1, db.Shows.ToList().Count());
                Assert.AreEqual(seriesCount + 1, db.Series.ToList().Count());
            }
        }

        [Test]
        public void IfNewSeriesIsAdded_DetailsAreCorrect()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                _methods.AddSeries("Test Show", "example.com", 5, 1, 0, 1, 3, "Testing");

                var selectedSeries = db.Shows.Include(s => s.Series).Where(s => s.Title == "Test Show").FirstOrDefault();

                Assert.AreEqual("example.com", selectedSeries.Url);
                Assert.AreEqual(5, selectedSeries.Score);
                Assert.AreEqual(1, selectedSeries.Type);
                Assert.AreEqual(0, selectedSeries.Status);
                Assert.AreEqual(1, selectedSeries.Series.Season);
                Assert.AreEqual(3, selectedSeries.Series.Episode);
                Assert.AreEqual("Testing", selectedSeries.Notes);
            }
        }
    }
}