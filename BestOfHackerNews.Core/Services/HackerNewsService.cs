namespace BestOfHackerNews.Core.Services;

using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;

internal class HackerNewsService : IHackerNewsService
{
    public HackerNewsService(ICachedHttpClient cachedClient)
    {
    }

    public IList<Story> GetTopStories(int count)
    {
        throw new NotImplementedException();
    }
}
