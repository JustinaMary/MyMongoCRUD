using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MyMongoCRUD.ServiceModels.Models;

namespace MyMongoCRUD.Repositories
{
    public class MoviesRepository
    {
        private const int DefaultMoviesPerPage = 20;
        private const string DefaultSortKey = "title";
        private const int DefaultSortOrder = 1;
        private readonly IMongoCollection<Comment> _commentsCollection;
        private readonly IMongoCollection<Movie> _moviesCollection;
        private readonly IMongoClient _mongoClient;

        public MoviesRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _moviesCollection = mongoClient.GetDatabase("sample_mflix").GetCollection<Movie>("movies");
            _commentsCollection = mongoClient.GetDatabase("sample_mflix").GetCollection<Comment>("comments");
        }

        /// <summary>
        ///     Get a <see cref="Movie" /> with <see cref="Comment" /> collection
        /// </summary>
        public async Task<Movie> GetMovieAsync(string movieId, CancellationToken cancellationToken = default)
        {
            try
            {
                var movie = await _moviesCollection.Aggregate()
                                                   .Match(Builders<Movie>.Filter.Eq(x => x.Id, movieId))
                                                   // Add a lookup (left join) stage that includes the comments associated with the retrieved movie
                                                   .Lookup(_commentsCollection,
                                                           m => m.Id,
                                                           (Comment c) => c.MovieId,
                                                           (Movie m) => m.Comments)
                                                   .FirstOrDefaultAsync(cancellationToken);

                return movie;
            }
            catch (Exception ex)
            {
                // Catch the exception and check the exception type and message contents.
                // Return null if the exception is due to a bad/missing Id. Otherwise, throw.
                if (ex.GetType() == typeof(FormatException) && ex.Message.Contains("is not a valid 24 digit hex string"))
                    return null;

                throw;
            }
        }

        /// <summary>
        ///     Get a <see cref="IReadOnlyList{T}" /> of <see cref="Movie" /> documents from the repository.
        /// </summary>
        public async Task<IReadOnlyList<Movie>> GetMoviesAsync(int pageSize = DefaultMoviesPerPage, int page = 1,
                                                               string sortKey = DefaultSortKey,
                                                               int sortDirection = DefaultSortOrder,
                                                               CancellationToken cancellationToken = default)
        {
            var skip = pageSize * (page - 1);
            var limit = pageSize;

            var sortFilter = new BsonDocument(sortKey, sortDirection);
            var movies = await _moviesCollection.Find(Builders<Movie>.Filter.Empty)
                                                .Sort(sortFilter)
                                                .Skip(skip)
                                                .Limit(limit)
                                                .ToListAsync(cancellationToken);
            return movies;
        }

        /// <summary>
        ///     Finds all movies that match the provided genres, cast, directors, and countries
        /// </summary>
        /// <returns>A simpler list of Movies</returns>
        public async Task<IReadOnlyList<Movie>> GetMoviesFilteredOrderedPaged(string[] genres,
                                                                              string[] countries,
                                                                              string[] directors,
                                                                              string[] cast,
                                                                              string sortKey = DefaultSortKey,
                                                                              int sortDirection = DefaultSortOrder,
                                                                              int pageSize = DefaultMoviesPerPage,
                                                                              int page = 1,
                                                                              CancellationToken cancellationToken = default)
        {
            var pipeline = new List<BsonDocument>();

            if (genres != null && genres.Length > 0)
                pipeline.Add(new BsonDocument("$match", new BsonDocument("genres", new BsonDocument("$in", new BsonArray(genres)))));

            if (countries != null && countries.Length > 0)
                pipeline.Add(new BsonDocument("$match", new BsonDocument("countries", new BsonDocument("$in", new BsonArray(countries)))));

            if (directors != null && directors.Length > 0)
                pipeline.Add(new BsonDocument("$match", new BsonDocument("directors", new BsonDocument("$in", new BsonArray(directors)))));

            if (cast != null && cast.Length > 0)
                pipeline.Add(new BsonDocument("$match", new BsonDocument("cast", new BsonDocument("$in", new BsonArray(cast)))));

            pipeline.Add(new BsonDocument("$sort", new BsonDocument(sortKey, sortDirection)));

            pipeline.Add(new BsonDocument("$skip", pageSize * (page - 1)));

            pipeline.Add(new BsonDocument("$limit", pageSize));


            // Runs the pipeline built with Aggregate 
            var result = await _moviesCollection.Aggregate(PipelineDefinition<Movie, Movie>.Create(pipeline))
                                                .ToListAsync(cancellationToken);

            return result;
        }


        /// <summary>
        ///     Gets the total number of movies
        /// </summary>
        /// <returns>A Long</returns>
        public async Task<long> GetMoviesCountAsync()
        {
            return await _moviesCollection.CountDocumentsAsync(Builders<Movie>.Filter.Empty);
        }

        public async Task<IReadOnlyList<Movie>> UpdateMoviesRuntimeAsync(int Runtime)
        {
            //Create a session object that is used when leveraging transactions
            var session = _mongoClient.StartSession();
            var filter = new FilterDefinitionBuilder<Movie>().Gt(r => r.Runtime, Runtime);

            //Begin transaction
            session.StartTransaction();

            try
            {
                //Increase the runtime by +5 for movies whose runtime is greater than the passed Runtime parameter
                var update = new UpdateDefinitionBuilder<Movie>().Inc<Double>(r => r.Runtime, 5);
                await _moviesCollection.UpdateManyAsync(filter, update); //,options);

                //Made it here without error? Let's commit the transaction
                session.CommitTransaction();

                return await _moviesCollection.Find<Movie>(filter).ToListAsync();
            }
            catch (Exception)
            {
                session.AbortTransaction();
                throw;
            }
        }
    }
}
