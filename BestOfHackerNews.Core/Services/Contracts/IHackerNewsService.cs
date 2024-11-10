namespace BestOfHackerNews.Core.Services.Contracts;

using BestOfHackerNews.Core.Domain;

internal interface IHackerNewsService
{
    Task<IList<Story>> GetTopStories(int count);
}
