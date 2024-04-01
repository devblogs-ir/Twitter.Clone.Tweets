using System.Text.RegularExpressions;

namespace Twitter.Clone.Tweets.Extensions;

public class TextScanner
{
    public static List<string> GetMentions(string content)
    {
        var mentions = new List<string>();

        var regex = new Regex(@"(?<=@)\w+");

        var matches = regex.Matches(content);

        foreach (Match item in matches)
        {
            mentions.Add(item.Value);
        }

        return mentions;
    }
}
