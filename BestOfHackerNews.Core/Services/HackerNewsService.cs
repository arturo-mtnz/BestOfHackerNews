namespace BestOfHackerNews.Core.Services;

using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;

internal class HackerNewsService : IHackerNewsService
{
    public HackerNewsService(ICachedHackerNewsHttpClient cachedHackerNewsHttpClient)
    {
        this.CachedHackerNewsHttpClient = cachedHackerNewsHttpClient;
    }

    private ICachedHackerNewsHttpClient CachedHackerNewsHttpClient { get; }

    public async Task<IList<Story>> GetTopStories(int count)
    {
        IList<int> storiesIds = await this.CachedHackerNewsHttpClient.GetTopStoriesIds();
        IEnumerable<Task<Story>> storyTasks = storiesIds.Select(this.CachedHackerNewsHttpClient.GetStory);

        List<Story> stories = (await Task.WhenAll(storyTasks)).ToList();
        stories.Sort();
        return stories.Take(count).ToList();
    }
}
