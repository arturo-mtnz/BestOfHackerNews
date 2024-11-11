namespace BestOfHackerNews.Tests;

using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestOfHackerNews.Core;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Dto;

[TestClass]
public class MappingTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void Map_StoryApiDtoToStory_Correctly()
    {
        int unixTime = 1609459200; 
        int[] comments = { 1, 2, 3 };
        StoryApiDto storyApiDto = new()
        {
            Time = unixTime,
            Kids = comments,
            URL = "http://foo.com",
            By = "Arthur C. Doyle"
        };

        Story story = _mapper.Map<Story>(storyApiDto);

        Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime, story.Time);
        Assert.AreEqual(comments.Length, story.CommentCount);
        Assert.AreEqual(storyApiDto.URL, story.URI);
        Assert.AreEqual(storyApiDto.By, story.PostedBy);
    }

    [TestMethod]
    public void Set_CommentCountToZero_WhenKidsIsNull()
    {
        StoryApiDto storyApiDto = new() 
        {
            Kids = null
        };

        Story story = _mapper.Map<Story>(storyApiDto);
        Assert.AreEqual(0, story.CommentCount);
    }
}
