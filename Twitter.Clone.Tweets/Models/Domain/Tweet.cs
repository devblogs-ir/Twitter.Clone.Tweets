namespace Twitter.Clone.Tweets.Models.Domain;

public class Tweet
{
    public int Id { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }
}
