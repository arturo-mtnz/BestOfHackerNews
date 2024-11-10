namespace BestOfHackerNews.Core.Api;

using BestOfHackerNews.Core.Services.Contracts;
using Microsoft.AspNetCore.Builder;

internal class ApiMapper : IApiMapper
{
    public ApiMapper(
        IHackerNewsService hackerNewsService
        )
    {
        this.HackerNewsService = hackerNewsService;
    }

    private IHackerNewsService HackerNewsService { get; }

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/topStories", (int count) =>
            {
                return this.HackerNewsService.GetTopStories(count);
            }
        );
    }
}
