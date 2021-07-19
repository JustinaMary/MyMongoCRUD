using System.Collections.Generic;
using MyMongoCRUD.Helpers;
using MyMongoCRUD.ServiceModels.Models;
using ServiceStack;

namespace MyMongoCRUD.ServiceModels
{

    [Route("/movies/{Id}", "GET")]
    public class GetMovie : IReturn<GetMovieResponse>
    {
        public string Id { get; set; }
    }

    public class GetMovieResponse : ResponseBase
    {
        public Movie Movie { get; set; }
    }

    [Route("/movies", "GET")]
    public class GetMovies : IReturn<GetMoviesResponse>
    {
        public int PageSize { get; set; } = 20;
        public int Page { get; set; } = 1;
        public string SortKey { get; set; } = "tomatoes.viewer.numReviews";
        public int SortDirection { get; set; } = -1;
    }

    public class GetMoviesResponse : ResponseBase
    {
        public IReadOnlyList<Movie> Movies { get; set; }
    }


    /// <summary>
    /// Filters movie collection using pipeline
    /// </summary>
    [Route("/filter", "POST")]
    public class GetFilteredMovies : IReturn<GetFilteredMoviesResponse>
    {
        public MovieFilter Filter { get; set; }
    }

    public class GetFilteredMoviesResponse : ResponseBase
    {
        public IReadOnlyList<Movie> Movies { get; set; }
    }



    /// <summary>
    /// To bulk update movies runtime using transactions feature
    /// </summary>
    [Route("/movies/{Runtime}", "PATCH")]
    public class UpdateMoviesRuntime : IReturn<GetUpdatedMoviesResponse>
    {
        public int Runtime { get; set; }
    }

    public class GetUpdatedMoviesResponse : ResponseBase
    {
        public IReadOnlyList<Movie> Movies { get; set; }
    }

}
