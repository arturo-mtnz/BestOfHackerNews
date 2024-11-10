namespace BestOfHackerNews.Core.Services.Contracts;

using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;

internal interface ICachedHttpClient
{
    IList<int> GetTopStoriesIds();
    Story GetStory();
}
