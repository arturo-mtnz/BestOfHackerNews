namespace BestOfHackerNews.Core.Services.Contracts;

using BestOfHackerNews.Core.Domain;

/// <summary>
/// Service for obtaining stories from Hacker News API.
/// </summary>
internal interface IHackerNewsService
{
    /// <summary>
    /// Retrieves the top stories from Hacker News
    /// </summary>
    /// <param name="count">The maximum number of stories to retrieve</param>
    /// <returns>The list of top stories with highest score</returns>
    Task<IList<Story>> GetTopStories(int count);
}
