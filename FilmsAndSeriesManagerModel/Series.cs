using System;
using System.Collections.Generic;

#nullable disable

namespace FilmsAndSeriesManagerModel
{
    public partial class Series
    {
        public int? ShowId { get; set; }
        public int? Season { get; set; }
        public int? Episode { get; set; }

        public virtual Show Show { get; set; }
    }
}
