using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MyMongoCRUD.Repositories
{
    public static class RepositoryExtensions
    {
        public static void RegisterMongoDbRepositories(this IServiceCollection servicesBuilder)
        {
            servicesBuilder.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>()["MongoDBUri"];
                return new MongoClient(uri);
            });
            servicesBuilder.AddSingleton<MoviesRepository>();
            servicesBuilder.AddSingleton<CommentsRepository>();
        }
    }
}
