

using ArcTouch.TestDevelopment.Service.ApiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcTouch.TestDevelopment.Service.ApiResponses
{
    public class UpcomingMoviesResponse : APIResponseBase<UpcomingMovies>
    {
        public List<ApiObjects.Movie> results { get; set; }
        public int page { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }

    }
}
