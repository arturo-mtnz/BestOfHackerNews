namespace BestOfHackerNews.Core.Services.Contracts;

using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;

internal interface ICachedHackerNewsHttpClient
{
    Task<IList<int>> GetTopStoriesIds();
    Task<Story> GetStory(int storyId);
}
