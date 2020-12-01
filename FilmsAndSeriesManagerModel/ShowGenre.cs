using System;
using System.Collections.Generic;

#nullable disable

namespace FilmsAndSeriesManagerModel
{
    public partial class ShowGenre
    {
        public int ShowId { get; set; }
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Show Show { get; set; }
    }
}
