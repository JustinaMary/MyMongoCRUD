using System.Collections.Generic;
using MyMongoCRUD.Helpers;
using MyMongoCRUD.ServiceModels.Models;
using Newtonsoft.Json;
using ServiceStack;

namespace MyMongoCRUD.ServiceModels
{

    [Route("/comments/{Id}", "GET")]
    public class GetComment : IReturn<GetCommentResponse>
    {
        public string Id { get; set; }
    }

    public class GetCommentResponse : ResponseBase
    {
        public Comment Comment { get; set; }
    }


    [Route("/movies/{MovieId}/comments", "GET")]
    public class GetComments : IReturn<GetCommentsResponse>
    {
        public string MovieId { get; set; }
    }

    public class GetCommentsResponse : ResponseBase
    {
        public IReadOnlyList<Comment> Comments { get; set; }
    }


    [Route("/comments")]
    public class AddComment : IReturn<AddCommentResponse>
    {
        public AddMovieCommentInput Comment { get; set; }
    }

    public class AddCommentResponse : ResponseBase
    {
        public Comment Comment { get; set; }
    }

    [Route("/comments/{Id}", "PATCH")]
    public class UpdateComment : IReturn<UpdateCommentResponse>
    {
        public string Id { get; set; }
        public UpdateMovieCommentInput Comment { get; set; }
    }

    public class UpdateCommentResponse : ResponseBase
    {
        public Comment Comment { get; set; }
    }

    [Route("/comments/{Id}/delete", "DELETE")]
    public class DeleteComment : IReturn<DeleteCommentResponse>
    {
        public string Id { get; set; }
    }

    public class DeleteCommentResponse : ResponseBase
    {
    }

}
