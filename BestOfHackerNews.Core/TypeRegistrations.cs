namespace BestOfHackerNews.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using BestOfHackerNews.Core.Api;
using BestOfHackerNews.Core.Services;
using BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Static class which provides intefaces' implementations registration.
/// </summary>
internal static class TypeRegistrations
{
    /// <summary>
    /// Registers implementations of interfaces which need to be used for DI.
    /// </summary>
    /// <param name="webAppBuilder">The WebApplicationBuilder object where we are registering the implementations</param>
    public static void RegisterCustomServices(WebApplicationBuilder webAppBuilder)
    {
        /*** Registration of implementated interfaces ***/

        /* Web API */
        webAppBuilder.Services.AddSingleton<IApiMapper, ApiMapper>();

        /* Services */
        webAppBuilder.Services.AddSingleton<IHackerNewsService, HackerNewsService>();
        webAppBuilder.Services.AddSingleton<IHackerNewsRepository, CachedHackerNewsRepository>();
        webAppBuilder.Services.AddSingleton<IHttpSimpleClient, HttpThrottledClient>();
    }
}
