using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SocialMedia.MongoDbEnt
{
    public class Comment
    {
        [BsonElement("user")]
        public string UserName { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
        public override string ToString()
        {
            return $"\nuser: {UserName}\n\n{Text}";
        }
    }
}