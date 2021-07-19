using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMongoCRUD.Helpers
{
    public class MovieFilter
    {
        public string[] CastFilter { get; set; } = null;        // comma seperated list or null
        // AND
        public string[] CountryFilter { get; set; } = null;     // comma seperated list or null
        // AND
        public string[] GenreFilter { get; set; } = null;       // comma seperated list or null
        // AND
        public string[] DirectorFilter { get; set; } = null;    // comma seperated list or null

        public int PageSize { get; set; } = 20;
        public int Page { get; set; } = 1;
        public string SortKey { get; set; } = "title";
        public int SortDirection { get; set; } = 1;
    }
}
