using System;
using MyMongoCRUD.Repositories;
using ServiceStack;

namespace MyMongoCRUD.ServiceInterfaces
{
    public partial class MyServices : Service
    {
        private readonly MoviesRepository _movieRepository;
        private readonly CommentsRepository _commentsRepository;

        public MyServices(CommentsRepository commentsRepository, MoviesRepository moviesRepository)
        {
            _commentsRepository = commentsRepository;
            _movieRepository = moviesRepository;
        }

        internal ResponseStatus GetResponseStatus(Exception ex)
        {
            return new ResponseStatus
            {
                ErrorCode = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
        }
    }
}
