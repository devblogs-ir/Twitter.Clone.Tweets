
using System.Threading.Channels;
using Twitter.Clone.Tweets.Models.Contracts;

namespace Twitter.Clone.Tweets.BackgroundServices.Channel;

public class ReaderBackgroundService : BackgroundService
{
    private readonly Channel<CreateTweetContext> _channel;
    public string Type;
    public ReaderBackgroundService(Channel<CreateTweetContext> channel, string type)
    {
        _channel = channel;
        Type = type;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var item in _channel.Reader.ReadAllAsync())
            {
            }
        }
    }
}
