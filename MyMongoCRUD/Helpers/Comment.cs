using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMongoCRUD.Helpers
{
    /// <summary>
    ///     Helper class to define the expected JSON object passed in the
    ///     Request body.
    /// </summary>
    public class AddMovieCommentInput
    {
        public string MovieId { get; set; }

        public string Comment { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }

    public class UpdateMovieCommentInput
    {
        public string UpdatedComment { get; set; }

        public string Email { get; set; }
    }

    public class DeleteMovieCommentInput
    {
        public string MovieId { get; set; }

        public string Email { get; set; }
    }
}
