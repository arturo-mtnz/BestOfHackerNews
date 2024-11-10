﻿namespace BestOfHackerNews.Core.Model;

using System;

internal class Story
{
    public string Title { get; set; } = string.Empty;
    public string URI { get; set; } = string.Empty;
    public string PostedBy { get; set; } = string.Empty;
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }
}
