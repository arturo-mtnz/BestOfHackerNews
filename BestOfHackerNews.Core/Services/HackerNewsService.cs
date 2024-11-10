namespace BestOfHackerNews.Core.Services;

using System.Collections.Generic;
using BestOfHackerNews.Core.Model;
using BestOfHackerNews.Core.Services.Contracts;

internal class HackerNewsService : IHackerNewsService
{
    public HackerNewsService()
    {
    }

    public IList<Story> GetTopStories(int count)
    {
        throw new NotImplementedException();
    }
}
