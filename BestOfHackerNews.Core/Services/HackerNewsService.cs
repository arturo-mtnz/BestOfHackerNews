namespace BestOfHackerNews.Core.Services;

using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Implementation of IHackerNewsService providing sorting of stories.
/// </summary>
internal class HackerNewsService : IHackerNewsService
{
    public HackerNewsService(IHackerNewsRepository cachedHackerNewsRepository)
    {
        this.CachedHackerNewsRepository = cachedHackerNewsRepository;
    }

    private IHackerNewsRepository CachedHackerNewsRepository { get; }

    public async Task<IList<Story>> GetTopStories(int count)
    {
        IList<int> storiesIds = await this.CachedHackerNewsRepository.GetTopStoriesIds();
        IEnumerable<Task<Story>> storyTasks = storiesIds.Select(this.CachedHackerNewsRepository.GetStory);

        List<Story> stories = (await Task.WhenAll(storyTasks)).ToList();
        stories.Sort();
        return stories.Take(count).ToList();
    }
}
