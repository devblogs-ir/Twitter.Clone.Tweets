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
            if (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                while (_channel.Reader.TryRead(out var item))
                {
                    var mentions = _textScanner.GetMentions(item.Content);

                    var hashtags = _textScanner.GetHashtags(item.Content);

                    //Do something with mentions and hashtags
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
