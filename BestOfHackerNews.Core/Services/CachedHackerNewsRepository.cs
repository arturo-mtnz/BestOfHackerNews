namespace BestOfHackerNews.Core.Services;

using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Dto;
using BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Implementation of IHackerNewsRepository providing caching capabilities.
/// </summary>
internal class CachedHackerNewsRepository : IHackerNewsRepository
{
    public CachedHackerNewsRepository(IMemoryCache memorycache, IMapper mapper, IHttpSimpleClient httpSimpleClient)
    {
        this.MemoryCache = memorycache;
        this.Mapper = mapper;
        this.HttpSimpleClient = httpSimpleClient;

        this.PseudoRandomGenerator = new Random();
    }

    private IMemoryCache MemoryCache { get; }

    private IMapper Mapper { get; }

    private IHttpSimpleClient HttpSimpleClient { get; }

    private Random PseudoRandomGenerator { get; }

    public async Task<IList<int>> GetTopStoriesIds()
    {
        const int expirationInSeconds = 10;
        const string cacheKey = "TopStoriesIds";

        IList<int>? topStoriesIds;
        bool cached = this.MemoryCache.TryGetValue(cacheKey, out topStoriesIds);
        if (cached && topStoriesIds is not null)
        {
            return topStoriesIds;
        }

        topStoriesIds = await this.GetTopStoriesIdsFromApi();

        this.MemoryCache.Set(
            cacheKey,
            topStoriesIds,
            TimeSpan.FromSeconds(expirationInSeconds));
        return topStoriesIds;
    }

    public async Task<Story> GetStory(int storyId)
    {
        string cacheKey = $"Story_{storyId}";

        Story? story;
        bool cached = this.MemoryCache.TryGetValue(cacheKey, out story);
        if (cached && story is not null)
        {
            return story;
        }

        StoryApiDto storyDto = await this.GetStoryFromApi(storyId);
        story = this.Mapper.Map<Story>(storyDto);

        this.MemoryCache.Set(
            cacheKey,
            story,
            TimeSpan.FromSeconds(this.GetStoryCacheExpiration()));
        return story;
    }

    private int GetStoryCacheExpiration()
    {
        const int minExpirationInSeconds = 30;
        const int maxExpirationInSeconds = 60;

        return this.PseudoRandomGenerator.Next(minExpirationInSeconds, maxExpirationInSeconds);
    }

    private async Task<IList<int>> GetTopStoriesIdsFromApi()
    {
        const string topStoriesUri = "https://hacker-news.firebaseio.com/v0/beststories.json";
        return await this.HttpSimpleClient.GetAsync<IList<int>>(topStoriesUri);
    }

    private async Task<StoryApiDto> GetStoryFromApi(int storyId)
    {
        string storyUri = $"https://hacker-news.firebaseio.com/v0/item/{storyId}.json";

        return await this.HttpSimpleClient.GetAsync<StoryApiDto>(storyUri);
    }
}
