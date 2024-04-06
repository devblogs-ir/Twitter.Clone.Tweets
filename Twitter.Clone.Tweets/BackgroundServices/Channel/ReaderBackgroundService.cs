using System.Threading.Channels;
using Twitter.Clone.Tweets.Helpers;
using Twitter.Clone.Tweets.Models.Contracts;

namespace Twitter.Clone.Tweets.BackgroundServices.Channel;

public class ReaderBackgroundService(Channel<CreateTweetContext> channel, TextScanner textScanner) : BackgroundService
{
    private readonly Channel<CreateTweetContext> _channel = channel;
    private readonly TextScanner _textScanner = textScanner;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var item in _channel.Reader.ReadAllAsync())
            {
                var mentions = _textScanner.GetMentions(item.Content);

                var hashtags = _textScanner.GetHashtags(item.Content);


            }
        }
    }
}
