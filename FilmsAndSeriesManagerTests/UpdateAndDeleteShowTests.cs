using NUnit.Framework;
using FilmsAndSeriesManagerModel;
using FilmsAndSeriesManagerBusiness;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FilmsAndSeriesManagerTests
{
    class UpdateAndDeleteFilmTests
    {
        Methods _methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = "Test Show", Url = "", Type = 0, Score = 0, Status = 0, Notes = "" };
                db.Shows.Add(newFilm);
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
        public void UpdateFavouriteTest()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                _methods.SelectedShow = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();
                _methods.UpdateFavourite();
            }

            using (var db = new FilmsAndSeriesManagerContext())
            {
                var updatedFilm = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();

                Assert.AreEqual(true, updatedFilm.Favourite);
            }
        }

        [Test]
        public void WhenFilmDetailsAreChanged_TheDatabaseIsUpdated()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                _methods.SelectedShow = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();
                _methods.UpdateFilm("Test Show", "a.com", 9, 2, "This is a note.");
            }

            using (var db = new FilmsAndSeriesManagerContext())
            {
                var updatedFilm = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();

                Assert.AreEqual("a.com", updatedFilm.Url);
                Assert.AreEqual(9, updatedFilm.Score);
                Assert.AreEqual(2, updatedFilm.Status);
                Assert.AreEqual("This is a note.", updatedFilm.Notes);
            }
        }

        [Test]
        public void WhenAFilmIsDeleted_ShowsCountDecreasesBy1()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                int count = db.Shows.ToList().Count();
                _methods.SelectedShow = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();
                _methods.DeleteShow();

                Assert.AreEqual(count - 1, db.Shows.ToList().Count());
            }
        }
    }

    class UpdateAndDeleteSeriesTests
    {
        Methods _methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newFilm = new Show { Title = "Test Show", Url = "", Type = 1, Score = 0, Status = 0, Notes = "" };
                db.Shows.Add(newFilm);
                db.SaveChanges();

                var selectedFilm = db.Shows.Where(f => f.Title == newFilm.Title).FirstOrDefault();
                var newSeries = new Series { Show = selectedFilm, Season = 0, Episode = 0 };
                db.Series.Add(newSeries);
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
        public void WhenSeriesDetailsAreChanged_TheDatabaseIsUpdated()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                _methods.SelectedShow = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();
                _methods.UpdateSeriesDetails(1, 3);

                var updatedSeries = db.Shows.Include(s => s.Series).Where(s => s.Title == "Test Show").FirstOrDefault();

                Assert.AreEqual(1, updatedSeries.Series.Season);
                Assert.AreEqual(3, updatedSeries.Series.Episode);
            }
        }

        [Test]
        public void WhenASeriesIsDeleted_ShowsAndSeriesCountDecreaseBy1()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                int countShows = db.Shows.ToList().Count();
                int countSeries = db.Series.ToList().Count();
                _methods.SelectedShow = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();
                _methods.DeleteShow();

                Assert.AreEqual(countShows - 1, db.Shows.ToList().Count());
                Assert.AreEqual(countSeries - 1, db.Series.ToList().Count());
            }
        }
    }
}
