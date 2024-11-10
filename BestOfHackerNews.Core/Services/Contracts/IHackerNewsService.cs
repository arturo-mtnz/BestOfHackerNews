namespace BestOfHackerNews.Core.Services.Contracts;

using BestOfHackerNews.Core.Domain;

internal interface IHackerNewsService
{
    IList<Story> GetTopStories(int count);
}
