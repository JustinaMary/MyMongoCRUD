using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MyMongoCRUD.Repositories;
using MyMongoCRUD.ServiceModels;
using MyMongoCRUD.ServiceModels.Models;
using ServiceStack;

namespace MyMongoCRUD.ServiceInterfaces
{
    public partial class MyServices : Service
    {
        public async Task<object> Get(GetMovie request)
        {
            try
            {
                var movie = await _movieRepository.GetMovieAsync(request.Id);
                return new GetMovieResponse
                {
                    Movie = movie
                };
            }
            catch (Exception ex)
            {
                return new GetMovieResponse
                {
                    Movie = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }
        }

        public async Task<object> Get(GetMovies request)
        {
            try
            {
                var movies = await _movieRepository.GetMoviesAsync(request.PageSize, request.Page, request.SortKey, request.SortDirection);
                return new GetMoviesResponse
                {
                    Movies = movies,
                };
            }
            catch (Exception ex)
            {
                return new GetMoviesResponse
                {
                    Movies = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }

        }

        public async Task<object> Post(GetFilteredMovies request)
        {
            try
            {
                var movies = await _movieRepository.GetMoviesFilteredOrderedPaged(request.Filter.GenreFilter,
                                                                                  request.Filter.CountryFilter,
                                                                                  request.Filter.DirectorFilter,
                                                                                  request.Filter.CastFilter,
                                                                                  request.Filter.SortKey, request.Filter.SortDirection,
                                                                                  request.Filter.PageSize, request.Filter.Page);
                return new GetFilteredMoviesResponse
                {
                    Movies = movies,
                };
            }
            catch (Exception ex)
            {
                return new GetFilteredMoviesResponse
                {
                    Movies = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }

        }

        public async Task<object> Patch(UpdateMoviesRuntime request)
        {
            try
            {
                var movies = await _movieRepository.UpdateMoviesRuntimeAsync(request.Runtime);

                return new GetUpdatedMoviesResponse
                {
                    Movies = movies
                };
            }
            catch (Exception ex)
            {
                return new GetFilteredMoviesResponse
                {
                    Movies = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }

        }

    }
}