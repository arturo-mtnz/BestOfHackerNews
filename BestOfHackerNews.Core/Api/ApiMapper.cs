namespace BestOfHackerNews.Core.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Maps exposed REST endpoints with their logic.
/// </summary>
internal class ApiMapper : IApiMapper
{
    public ApiMapper(
        IHackerNewsService hackerNewsService,
        ILogger<ApiMapper> logger
        )
    {
        this.HackerNewsService = hackerNewsService;
        this.Logger = logger;
    }

    private IHackerNewsService HackerNewsService { get; }

    private ILogger Logger { get; }

    /// <summary>
    /// Registers the available endpoints
    /// </summary>
    /// <param name="app">The WebApplication where the endpoints must be registered</param>
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/topStories",
            [SwaggerOperation(
                Summary = "Retrieves the top stories from Hacker News",
                Description = "This endpoint retrieves the top n stories from the Hacker News API, where n is specified by the count parameter."
            )]
            async (int count) => 
                {
                    return await this.GetTopStories(count);
                }
            )
            .Produces<IList<Story>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError); 
    }

    /// <summary>
    /// Retrieves the top stories from Hacker News
    /// </summary>
    /// <param name="count">The maximum number of stories to retrieve</param>
    /// <returns>IResult with HTTP Code 200 and the list of stories if succeeds,IResult with HTTP Code otherwise</returns>
    private async Task<IResult> GetTopStories(int count)
    {
        try
        {
            IList<Story> result = await this.HackerNewsService.GetTopStories(count);
            this.Logger.LogInformation($"{nameof(ApiMapper)}|{nameof(GetTopStories)}|Got {result.Count} top stories");

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"{nameof(ApiMapper)}|{nameof(GetTopStories)}|Error while obtaining the {count} top stories.");
            return Results.Problem("Error while getting top stories from Hacker News API");
        }
    }
}
