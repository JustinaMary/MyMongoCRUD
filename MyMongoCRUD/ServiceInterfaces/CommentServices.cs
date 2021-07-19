using System;
using System.Threading.Tasks;
using MyMongoCRUD.ServiceModels;
using ServiceStack;

namespace MyMongoCRUD.ServiceInterfaces
{
    public partial class MyServices : Service
    {
        public async Task<object> Get(GetComment request)
        {
            try
            {
                var comment = await _commentsRepository.GetCommentAsync(request.Id);
                return new GetCommentResponse
                {
                    Comment = comment
                };
            }
            catch (Exception ex)
            {
                return new GetCommentResponse
                {
                    Comment = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }
        }

        public async Task<object> Get(GetComments request)
        {
            try
            {
                var comments = await _commentsRepository.GetCommentsAsync(request.MovieId);
                return new GetCommentsResponse
                {
                    Comments = comments
                };

            }
            catch (Exception ex)
            {
                return new GetCommentsResponse
                {
                    Comments = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }
        }

        public async Task<object> Post(AddComment request)
        {
            try
            {
                var comment = await _commentsRepository.AddCommentAsync(request.Comment.Email, request.Comment.Name, request.Comment.MovieId, request.Comment.Comment);
                return new AddCommentResponse
                {
                    Comment = comment
                };

            }
            catch (Exception ex)
            {
                return new GetCommentsResponse
                {
                    Comments = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }
        }

        public async Task<object> Patch(UpdateComment request)
        {
            try
            {
                var comment = await _commentsRepository.UpdateCommentAsync(request.Id, request.Comment.Email, request.Comment.UpdatedComment);
                return new UpdateCommentResponse
                {
                    Comment = comment
                };

            }
            catch (Exception ex)
            {
                return new GetCommentsResponse
                {
                    Comments = null,
                    ResponseStatus = GetResponseStatus(ex)
                };
            }
        }

        public async Task<object> Delete(DeleteComment request)
        {
            try
            {
                await _commentsRepository.DeleteCommentAsync(request.Id);
                return new DeleteCommentResponse();
            }
            catch (Exception ex)
            {
                return new DeleteCommentResponse
                {
                    ResponseStatus = GetResponseStatus(ex)
                };
            }
        }
    }
}