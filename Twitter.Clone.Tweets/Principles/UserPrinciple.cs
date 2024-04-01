using Twitter.Clone.Tweets.Principles.Interface;

namespace Twitter.Clone.Tweets.Principles;

public class UserPrinciple(string ipAddress) : IUserPrinciple
{
    public string IpAddress { get; init; } = ipAddress;
}
