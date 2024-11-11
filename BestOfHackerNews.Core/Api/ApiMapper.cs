namespace BestOfHackerNews.Core.Api;

using BestOfHackerNews.Core.Domain;
using BestOfHackerNews.Core.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Maps exposed REST endpoints with their logic.
/// </summary>
internal class ApiMapper : IApiMapper
{
    public ApiMapper(
        IHackerNewsService hackerNewsService
        )
    {
        this.HackerNewsService = hackerNewsService;
    }

    private IHackerNewsService HackerNewsService { get; }


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
            Console.WriteLine($"{nameof(ApiMapper)}|{nameof(GetTopStories)}|Got {result.Count} top stories");

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{nameof(ApiMapper)}|{nameof(GetTopStories)}|Error: {ex}");
            return Results.Problem("Error while getting top stories from Hacker News API");
        }
    }
}
