using Neo4jClient;
using SocialMedia.Neo4jEnt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia
{
    public class Neo4J
    {
        static BoltGraphClient Client
        {
            get
            {
                BoltGraphClient client = new BoltGraphClient("neo4j+s://7d056342.databases.neo4j.io:8943", "neo4j");
                client.ConnectAsync().Wait();
                return client;
            }
        }
        private User User;
        public void Authtentification(string username, string pass)
        {
            var user = Client.Cypher
                .Match("(u:User { username: $un})")
                .WithParam("un", username)
                .Where("u.password= $pass")
                .WithParam("pass", pass)
                .Return(u => u.As<User>())
                .ResultsAsync.Result;
            User = user.ElementAt(0);
        }
        public void CreateUser(string userName, string firstName, string lastName, string password, string email)
        {
            var newUser = new User
                (
                userName,
                firstName,
                lastName,
                password,
                email
                );
            Client.Cypher
                .Create("(u:User $newUser)")
                .WithParam("newUser", newUser)
                .ExecuteWithoutResultsAsync().Wait();
        }
        public IEnumerable<Object> Relationship(string username)
        {
            var userWithFollower = Client.Cypher
                .Match("(u:User {username: $un})-[r]-> (f: User {username: $fn})")
                .WithParam("un", User.UserName)
                .WithParam("fn", username)
                .Return((u, f) => new
                {
                    User = u.As<User>(),
                    Follower = f.As<User>()
                })
                .ResultsAsync.Result;
            return userWithFollower;
        }
        public double shortestPath(string username)
        {
            var userWithFollowers = Client.Cypher
                .Match("sp = shortestPath((:User {username: $un})-[*]-(:User {username: $fn}))")
                .WithParam("un", User.UserName)
                .WithParam("fn", username)
                .Return(sp => sp.Length())
                .ResultsAsync.Result;
            return userWithFollowers.First();
        }
    }
}