namespace BestOfHackerNews.Core.Domain;

using System;

internal class Story : IComparable<Story>
{
    public string Title { get; set; } = string.Empty;
    public string URI { get; set; } = string.Empty;
    public string PostedBy { get; set; } = string.Empty;
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }

    public int CompareTo(Story? other)
    {
        if (other is null)
        {
            return -1;
        }

        return other.Score.CompareTo(this.Score);
    }
}
