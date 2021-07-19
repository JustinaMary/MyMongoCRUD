using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MyMongoCRUD.ServiceModels.Models;
using ServiceStack;

namespace MyMongoCRUD.Repositories
{
    public class CommentsRepository
    {
        private readonly IMongoCollection<Comment> _commentsCollection;

        public CommentsRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _commentsCollection = mongoClient.GetDatabase("sample_mflix").GetCollection<Comment>("comments");
        }

        /// <summary>
        ///     Get a <see cref="Comment" />
        /// </summary>
        public async Task<Comment> GetCommentAsync(string commentId, CancellationToken cancellationToken = default)
        {
            return await _commentsCollection.Find(c => c.Id == new ObjectId(commentId)).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        ///     Get a <see cref="IReadOnlyList{T}" /> of <see cref="Comment" /> documents for moviedId from the repository.
        /// </summary>
        public async Task<IReadOnlyList<Comment>> GetCommentsAsync(string movieId, CancellationToken cancellationToken = default)
        {
            return await _commentsCollection.Find(c => c.MovieId == new ObjectId(movieId)).ToListAsync(cancellationToken);
        }

        /// <summary>
        ///     Adds a comment.
        /// </summary>
        public async Task<Comment> AddCommentAsync(string email,
                                          string name,
                                          string movieId,
                                          string comment,
                                          CancellationToken cancellationToken = default)
        {
            var newComment = new Comment
            {
                Date = DateTime.UtcNow,
                Text = comment,
                Name = name,
                Email = email,
                MovieId = new ObjectId(movieId)
            };

            await _commentsCollection.InsertOneAsync(newComment, new InsertOneOptions(), cancellationToken);
            return await _commentsCollection.Find(c => c.Id == newComment.Id).SingleAsync(cancellationToken);
        }

        /// <summary>
        ///     Updates an existing comment.
        /// </summary>
        public async Task<Comment> UpdateCommentAsync(string commentId,
                                                      string email,
                                                      string updatedComment,
                                                      CancellationToken cancellationToken = default)
        {
            var updateResult = await _commentsCollection.UpdateOneAsync(Builders<Comment>.Filter.Where(c => c.Email == email && c.Id == new ObjectId(commentId)),
                                                                        Builders<Comment>.Update.Set(c => c.Text, updatedComment).Set(c => c.Date, DateTime.UtcNow),
                                                                        new UpdateOptions { IsUpsert = false }, cancellationToken);

            if (!updateResult.IsAcknowledged)
                throw new ApplicationException("Not updated! " + updateResult.SerializeToString());

            return await _commentsCollection.Find(c => c.Id == new ObjectId(commentId)).SingleAsync(cancellationToken);
        }

        /// <summary>
        ///     Deletes a comment.
        /// </summary>
        public async Task DeleteCommentAsync(string commentId,
                                             CancellationToken cancellationToken = default)
        {
            await _commentsCollection.DeleteOneAsync(Builders<Comment>.Filter.Where(c => c.Id == new ObjectId(commentId)), cancellationToken);
        }
    }
}
