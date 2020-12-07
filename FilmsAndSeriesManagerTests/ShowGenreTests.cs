using NUnit.Framework;
using FilmsAndSeriesManagerModel;
using FilmsAndSeriesManagerBusiness;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FilmsAndSeriesManagerTests
{
    class ShowGenreTests
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
                if (selectedFilm.FirstOrDefault() != null)
                {
                    var showGenres = db.ShowGenres.Where(s => s.ShowId == selectedFilm.FirstOrDefault().Id);
                    foreach (var item in showGenres)
                    {
                        db.ShowGenres.Remove(item);
                    }
                }
                foreach (var film in selectedFilm)
                {
                    db.Shows.Remove(film);
                }
                db.SaveChanges();
            }
        }

        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 0, 1, 2 }, 3)]
        public void WhenGenreIsAddedToShow_TheDatabaseIsUpdated(int[] genreList, int expected)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedShow = db.Shows.Where(s => s.Title == "Test Show").FirstOrDefault();
                selectedShow.AddGenres(genreList.ToList());

                int count = db.ShowGenres.Where(s => s.ShowId == selectedShow.Id).ToList().Count();
                Assert.AreEqual(expected, count);
            }
        }
    }
}
