namespace BestOfHackerNews.Core.Services;

using System;
using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Dto;
using BestOfHackerNews.Core.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

internal class CachedHackerNewsHttpClient : ICachedHackerNewsHttpClient
{
    public CachedHackerNewsHttpClient(IMemoryCache memorycache, IMapper mapper)
    {
        this.MemoryCache = memorycache;
        this.Mapper = mapper;
        this.HackerNewsHttpClient = new HttpClient();
        this.PseudoRandom = new Random();
    }

    private IMemoryCache MemoryCache { get; }

    private IMapper Mapper { get; }

    private HttpClient HackerNewsHttpClient { get; }

    private Random PseudoRandom { get; }

    public async Task<IList<int>> GetTopStoriesIds()
    {
        const int expirationInSeconds = 300;
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

    private async Task<IList<int>> GetTopStoriesIdsFromApi()
    {
        const string topStoriesUri = "https://hacker-news.firebaseio.com/v0/beststories.json";

        HttpResponseMessage response = await this.HackerNewsHttpClient.GetAsync(topStoriesUri);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<List<int>>(content, serializerOptions) 
            ?? throw new InvalidOperationException("Failed to deserialize the top stories IDs received from the Hacker News API.");
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
        const int minExpirationInSeconds = 90;
        const int maxExpirationInSeconds = 120;

        return this.PseudoRandom.Next(minExpirationInSeconds, maxExpirationInSeconds);
    }

    private async Task<StoryApiDto> GetStoryFromApi(int storyId)
    {
        string storyUri = $"https://hacker-news.firebaseio.com/v0/item/{storyId}.json";

        HttpResponseMessage response = await this.HackerNewsHttpClient.GetAsync(storyUri);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<StoryApiDto>(content, serializerOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize the story {storyId} received from the Hacker News API.");
    }
}
