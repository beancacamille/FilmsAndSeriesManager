using NUnit.Framework;
using FilmsAndSeriesManagerModel;
using FilmsAndSeriesManagerBusiness;
using System.Linq;

namespace FilmsAndSeriesManagerTests
{
    public class AddFilmTest
    {
        Methods methods = new Methods();

        [SetUp]
        public void Setup()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var selectedFilm = db.Shows.Where(f => f.Title == "Detective Conan");
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
                methods.AddFilm("Detective Conan", "", 0, 0);

                Assert.AreEqual(count + 1, db.Shows.ToList().Count());
            }
        }
    }
}