namespace BestOfHackerNews.Core.Services;

using System;
using System.Collections.Generic;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

internal class CachedHackerNewsHttpClient : ICachedHttpClient
{
    public CachedHackerNewsHttpClient(IMemoryCache memorycache)
    {
        this.Memorycache = memorycache;
        this.HackerNewsHttpClient = new HttpClient();
    }

    private IMemoryCache Memorycache { get; }

    private HttpClient HackerNewsHttpClient { get; }

    public Story GetStory()
    {
        throw new NotImplementedException();
    }

    public IList<int> GetTopStoriesIds()
    {
        throw new NotImplementedException();
    }
}
