namespace BestOfHackerNews.Tests;

using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Dto;
using BestOfHackerNews.Core.Services;
using BestOfHackerNews.Core.Services.Contracts;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

[TestClass]
public class CachingTests
{
    private Mock<IMapper> _mapperMock;
    private Mock<IHttpSimpleClient> _httpClientMock;
    private Mock<ILogger<CachedHackerNewsRepository>> _logger;
    private CachedHackerNewsRepository _repository;
    private MemoryCache _memoryCache;

    [TestInitialize]
    public void SetUp()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _mapperMock = new Mock<IMapper>();
        _httpClientMock = new Mock<IHttpSimpleClient>();
        _logger = new Mock<ILogger<CachedHackerNewsRepository>>();

        _repository = new CachedHackerNewsRepository(
            _memoryCache,
            _mapperMock.Object,
            _httpClientMock.Object,
            _logger.Object);
    }

    [TestMethod]
    public async Task GetTopStoriesIds_ReturnsIds_FromApi_WhenNotCached()
    {
        int[] expectedIds = { 1, 2, 3 };

        _httpClientMock
            .Setup(client => client.GetAsync<IList<int>>(It.IsAny<string>()))
            .ReturnsAsync(expectedIds);

        IList<int> result = await _repository.GetTopStoriesIds();
        Assert.AreEqual(expectedIds, result);
    }

    [TestMethod]
    public async Task GetTopStoriesIds_ReturnsIds_FromCache_WhenCached()
    {
        int[] expectedIds = { 1, 2, 3 };
        _memoryCache.Set("TopStoriesIds", expectedIds);

        IList<int> result = await _repository.GetTopStoriesIds();

        Assert.AreEqual(expectedIds, result);
        _httpClientMock.Verify(client => client.GetAsync<IList<int>>(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task GetStory_ReturnsStory_FromApi_WhenNotCached()
    {
        const int storyId = 1;
        const string author = "Donald J. Trump";
        const string title = "My tariffs and me";

        StoryApiDto storyApiDto = new () { Id = 1, By = author, Title = title };
        Story expectedStory = new () { PostedBy = author, Title = title };

        _httpClientMock
            .Setup(client => client.GetAsync<StoryApiDto>(It.IsAny<string>()))
            .ReturnsAsync(storyApiDto);

        _mapperMock
            .Setup(mapper => mapper.Map<Story>(storyApiDto))
            .Returns(expectedStory);

        Story result = await _repository.GetStory(storyId);

        Assert.AreEqual(expectedStory, result);
        Assert.IsTrue(_memoryCache.TryGetValue($"Story_{storyId}", out var cachedStory));
        Assert.AreEqual(expectedStory, cachedStory);
    }

    [TestMethod]
    public async Task GetStory_ReturnsStory_FromCache_WhenCached()
    {
        const int storyId = 1;
        const string author = "Donald J. Trump";
        const string title = "My tariffs and me";

        Story expectedStory = new() { PostedBy = author, Title = title };
        _memoryCache.Set($"Story_{storyId}", expectedStory);

        Story result = await _repository.GetStory(storyId);

        Assert.AreEqual(expectedStory, result);
        _httpClientMock.Verify(client => client.GetAsync<StoryApiDto>(It.IsAny<string>()), Times.Never);
    }
}
