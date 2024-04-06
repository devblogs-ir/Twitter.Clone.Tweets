using System.Text.RegularExpressions;

namespace Twitter.Clone.Tweets.Helpers;

public partial class TextScanner
{
    public List<string> GetMentions(string content)
    {
        var mentions = new List<string>();

        var regex = MentionsRegex();

        var matches = regex.Matches(content);

        foreach (Match item in matches)
        {
            mentions.Add(item.Value);
        }

        return mentions;
    }

    public List<string> GetHashtags(string content)
    {
        var hashtags = new List<string>();

        var regex = HashtagsRegex();

        var matches = regex.Matches(content);

        foreach (Match item in matches)
        {
            hashtags.Add(item.Value);
        }

        return hashtags;
    }

    [GeneratedRegex(@"(?<=@)\w+")]
    private static partial Regex MentionsRegex();

    [GeneratedRegex(@"(?<=#)\w+")]
    private static partial Regex HashtagsRegex();
}
