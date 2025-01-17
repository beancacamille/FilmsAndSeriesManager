﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmsAndSeriesManagerModel
{
    public partial class Show
    {
        public List<Genre> GetAllGenre()
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var genreList = new List<Genre>();
                var showGenreQuery = db.ShowGenres.Include(s => s.Genre).Where(s => s.ShowId == Id);

                foreach (var item in showGenreQuery)
                {
                    genreList.Add(item.Genre);
                }
                return genreList;
            }
        }

        public List<int> GetAllGenreInt()
        {
            var genreListInt = new List<int>();
            GetAllGenre().ForEach(x => genreListInt.Add(x.Id));
            return genreListInt;
        }

        public string GetAllGenreString()
        {
            var genreListString = new StringBuilder();
            GetAllGenre().ForEach(x => genreListString.Append(x.Name + ", "));
            return genreListString.Length > 0 ? genreListString.ToString().Substring(0, genreListString.Length - 2) : "";
        }

        public void AddGenres(List<int> genreList)
        {
            using (var db = new FilmsAndSeriesManagerContext())
            {
                var thisShow = db.Shows.Include(s => s.ShowGenres).Where(s => s.Id == Id).FirstOrDefault();

                genreList.ForEach(x =>
                {
                    var selectedGenre = db.Genres.Where(g => g.Id == x).FirstOrDefault();
                    var newShowGenre = new ShowGenre { Show = thisShow, Genre = selectedGenre };

                    thisShow.ShowGenres.Add(newShowGenre);
                });

                db.SaveChanges();
            }
        }

        public override string ToString()
        {
            return $"{Score}\t{(Type == 0 ? "Film" : "Series")}\t{Title}";
        }
    }

    public partial class ShowStatus
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class Genre
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
