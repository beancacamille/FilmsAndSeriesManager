using System;
using System.Collections.Generic;

#nullable disable

namespace FilmsAndSeriesManagerModel
{
    public partial class Genre
    {
        public Genre()
        {
            ShowGenres = new HashSet<ShowGenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ShowGenre> ShowGenres { get; set; }
    }
}
