using MongoDB.Driver;
using SocialMedia.MongoDbEnt;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia
{
    public class MongoDB
    {
        private User User;
        private IMongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<User> usersCollection;
        private IMongoCollection<Post> postsCollection;

        public CommandMongoDB()
        {
            client = new MongoClient(ConfigurationBuilder().AddJsonFile(@"F:\Studying\Нереляційні БД\SocialMedia\SocialMedia\appsettings.json").Build().GetConnectionString("SN-mongodb"));
            db = client.GetDatabase("SocialMediaDB");
            usersCollection = db.GetCollection<User>("users");
            postsCollection = db.GetCollection<Post>("posts");
        }
        public bool Authtentification(string username, string pass)
        {
            var documents = usersCollection.Find(_ => true).ToListAsync();
            var filter = Builders<User>.Filter.Eq("username", username) & Builders<User>.Filter.Eq("password", pass);
            var found = usersCollection.Find(filter).ToList();
            if (found.Count != 0)
            {
                User = found[0];
                return true;
            }
            return false;
        }
        public void NewUser(string userName, string firstName, string lastName, string password, string email, List<string> follows)
        {
            var newUser = new User
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Follows = follows
            };

            usersCollection.InsertOne(newUser);
        }
        public List<Post> ShowPosts(string username)
        {
            var f = Builders<Post>.Filter.Eq("username", username);
            var posts = postsCollection.Find(f).Sort("{date : -1}").ToList();
            return posts;
        }
        public void CreateComment(Post post, string comment)
        {
            post.Comments.Add(new Comment { UserName = User.UserName, Text = comment});
            postsCollection.ReplaceOne(p => p.Id == post.Id, post);
        }
        public List<User> ShowFollows()
        {
            var f = Builders<User>.Filter.In("username", User.Follows);
            var follows = usersCollection.Find(f).ToList();
            return follows;
        }
    }
}