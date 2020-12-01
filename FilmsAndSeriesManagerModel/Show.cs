using System;
using System.Collections.Generic;

#nullable disable

namespace FilmsAndSeriesManagerModel
{
    public partial class Show
    {
        public Show()
        {
            ShowGenres = new HashSet<ShowGenre>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal? Score { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public bool? Favourite { get; set; }
        public string Notes { get; set; }

        public virtual ShowStatus StatusNavigation { get; set; }
        public virtual Series Series { get; set; }
        public virtual ICollection<ShowGenre> ShowGenres { get; set; }
    }
}
