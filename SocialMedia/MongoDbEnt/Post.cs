using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SocialMedia.MongoDbEnt
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("user")]
        public string User { get; set; }
        [BsonElement("post")]
        public string PostText { get; set; }
        [BsonElement("comment")]
        public List<Comment> Comments { get; set; }

        public override string ToString()
        {
            return $"\n\nuser: {User}  \n comments: {Comments.Count}\n\n" + PostText + "\n\n";
        }
    }
}