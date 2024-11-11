using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using BestOfHackerNews.Core;
using BestOfHackerNews.Core.Api;
using BestOfHackerNews.Core.Services.Contracts;
using BestOfHackerNews.Core.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder webAppBuilder = GetApplicationBuilder();
        WebApplication webApp = webAppBuilder.Build();

        /* Instantiate required services */
        IApiMapper apiMapper = webApp.Services.GetRequiredService<IApiMapper>();

        ConfigureWebApp(webApp, apiMapper);
        webApp.Run();
    }

    private static WebApplicationBuilder GetApplicationBuilder()
    {
        WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder();

        /* Register all custom interfaces implementations to make them usable via DI */
        TypeRegistrations.RegisterCustomServices(webAppBuilder);

        webAppBuilder.Services.AddEndpointsApiExplorer();
        webAppBuilder.Services.AddSwaggerGen(c => c.EnableAnnotations());
        webAppBuilder.Services.AddMemoryCache();
        webAppBuilder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        /* HTTP resilience and retries */
        webAppBuilder.Services.AddHttpClient<IHttpSimpleClient, HttpSimpleClient>()
            .AddStandardResilienceHandler();

        return webAppBuilder;
    }

    private static void ConfigureWebApp(WebApplication webApp, IApiMapper apiMapper)
    {
        webApp.UseSwagger();
        webApp.UseSwaggerUI();

        /* Register endpoints */
        apiMapper.RegisterEndpoints(webApp);
    }
}