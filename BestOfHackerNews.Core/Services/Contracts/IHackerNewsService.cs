namespace BestOfHackerNews.Core.Services.Contracts;

using BestOfHackerNews.Core.Model;

internal interface IHackerNewsService
{
    IList<Story> GetTopStories(int count);
}
