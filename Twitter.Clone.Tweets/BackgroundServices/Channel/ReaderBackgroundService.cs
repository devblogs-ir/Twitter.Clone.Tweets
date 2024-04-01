using System.Threading.Channels;
using Twitter.Clone.Tweets.Extensions;
using Twitter.Clone.Tweets.Models.Contracts;

namespace Twitter.Clone.Tweets.BackgroundServices.Channel;

public class ReaderBackgroundService(Channel<CreateTweetContext> channel) : BackgroundService
{
    private readonly Channel<CreateTweetContext> _channel = channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var item in _channel.Reader.ReadAllAsync())
            {
                var mentions = TextScanner.GetMentions(item.Content);
            }
        }
    }
}
