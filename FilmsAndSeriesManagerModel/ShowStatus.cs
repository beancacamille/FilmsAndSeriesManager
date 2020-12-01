using System;
using System.Collections.Generic;

#nullable disable

namespace FilmsAndSeriesManagerModel
{
    public partial class ShowStatus
    {
        public ShowStatus()
        {
            Shows = new HashSet<Show>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Show> Shows { get; set; }
    }
}
