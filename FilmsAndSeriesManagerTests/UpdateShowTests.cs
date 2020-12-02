using NUnit.Framework;
using FilmsAndSeriesManagerModel;
using FilmsAndSeriesManagerBusiness;
using System.Linq;

namespace FilmsAndSeriesManagerTests
{
    class UpdateShowTests
    {
        Methods methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var newShow = new Show { Title = "Butler", Url = "", Type = 0, Score = 10, Status = 3 };
                db.Shows.Add(newShow);
                db.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Butler");
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
                methods.SelectedShow = db.Shows.Where(s => s.Title == "Butler").FirstOrDefault();
                methods.UpdateFavourite();
                var show = db.Shows.Where(s => s.Title == "Butler").FirstOrDefault();

                Assert.AreEqual(true, show.Favourite);
            }
        }

        [Test]
        public void UpdateShowDetailsTest()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                methods.SelectedShow = db.Shows.Where(s => s.Title == "Butler").FirstOrDefault();
                methods.UpdateFilm("Butler", "a.com", 9, 2, "This is a note.");
                var show = db.Shows.Where(s => s.Title == "Butler").FirstOrDefault();

                Assert.AreEqual("a.com", show.Url);
                Assert.AreEqual(9, show.Score);
                Assert.AreEqual(2, show.Status);
                Assert.AreEqual("This is a note.", show.Notes);
            }
        }
    }
}
