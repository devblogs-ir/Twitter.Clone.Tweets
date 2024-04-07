using System.Threading.Channels;
using Twitter.Clone.Tweets.Helpers;
using Twitter.Clone.Tweets.Models.Contracts;

namespace Twitter.Clone.Tweets.BackgroundServices.Channel;

public class ReaderBackgroundService(Channel<CreateTweetContext> channel, TextScanner textScanner, ILogger<ReaderBackgroundService> logger) : BackgroundService
{
    private readonly Channel<CreateTweetContext> _channel = channel;
    private readonly TextScanner _textScanner = textScanner;
    private readonly ILogger<ReaderBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ReaderBackgroundService is starting.");

        try
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

                        _logger.LogInformation("processed successfully.");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing items.");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ReaderBackgroundService is stopping.");
        await base.StopAsync(cancellationToken);
    }
}