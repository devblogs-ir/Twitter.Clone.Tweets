using MongoDB.Bson;

namespace Twitter.Clone.Tweets.Models.Contracts;

public record class GetTweetRequest(string text, Guid userId, string objectId)
{
}