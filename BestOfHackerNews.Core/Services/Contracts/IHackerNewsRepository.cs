namespace BestOfHackerNews.Core.Services.Contracts;

using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;

/// <summary>
/// A read-only repository abstraction for stories from Hacker News API.
/// </summary>
internal interface IHackerNewsRepository
{
    /// <summary>
    /// Obtains the top stories ids
    /// </summary>
    /// <returns>The ids of the top stories</returns>
    Task<IList<int>> GetTopStoriesIds();

    /// <summary>
    /// Obtains a single story details
    /// </summary>
    /// <param name="storyId">The id of the srtory to retrieve</param>
    /// <returns>The story with given storyId</returns>
    Task<Story> GetStory(int storyId);
}
