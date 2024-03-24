using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ThirdParty.Json.LitJson;

namespace Twitter.Clone.Tweets.Models.Domain;

public class Tweet
{
   
    public ObjectId Id { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }
}
