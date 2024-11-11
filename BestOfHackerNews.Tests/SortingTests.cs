namespace BestOfHackerNews.Tests;

using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;
using BestOfHackerNews.Core.Services;

[TestClass]
public class SortingTests
{
    private Mock<IHackerNewsRepository> _repositoryMock;
    private HackerNewsService _service;

    [TestInitialize]
    public void SetUp()
    {
        _repositoryMock = new Mock<IHackerNewsRepository>();
        _service = new HackerNewsService(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task GetTopStories_Returns_StoriesSortedByScoreDescending()
    {
        List<int> storyIds = [1, 2, 3, 4, 5];

        List<Story> unsortedStories =
        [
            new Story { Score = 10, Title = "Story 1" },
            new Story { Score = 50, Title = "Story 2" },
            new Story { Score = 30, Title = "Story 3" },
            new Story { Score = 20, Title = "Story 4" },
            new Story { Score = 40, Title = "Story 5" }
        ];

        _repositoryMock
            .Setup(repo => repo.GetTopStoriesIds())
            .ReturnsAsync(storyIds);

        _repositoryMock
            .Setup(repo => repo.GetStory(It.IsAny<int>()))
            .ReturnsAsync((int id) => unsortedStories[id - 1]);

        IList<Story> result = await _service.GetTopStories(unsortedStories.Count);
        List<Story> expectedOrder = unsortedStories.OrderByDescending(s => s.Score).ToList();

        Assert.AreEqual(result.First().Score, 50);

        for (int i = 0; i < result.Count; i++)
        {
            Assert.AreEqual(result[i].Title, expectedOrder[i].Title);
        }
    }

    [TestMethod]
    public async Task GetTopStories_Returns_CorrectStoryNumber()
    {
        List<int> storyIds = [1, 2, 3, 4, 5];

        List<Story> unsortedStories =
        [
            new Story { Score = 10, Title = "Story 1" },
            new Story { Score = 50, Title = "Story 2" },
            new Story { Score = 30, Title = "Story 3" },
            new Story { Score = 20, Title = "Story 4" },
            new Story { Score = 40, Title = "Story 5" }
        ];

        _repositoryMock
            .Setup(repo => repo.GetTopStoriesIds())
            .ReturnsAsync(storyIds);

        _repositoryMock
            .Setup(repo => repo.GetStory(It.IsAny<int>()))
            .ReturnsAsync((int id) => unsortedStories[id - 1]);

        IList<Story> result = await _service.GetTopStories(3);

        Assert.AreEqual(result.First().Score, 50);
        Assert.AreEqual(result.Last().Score, 30);
        Assert.AreEqual(result.Count, 3);
    }
}
