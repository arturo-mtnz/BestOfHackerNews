using BestOfHackerNews.Core;
using BestOfHackerNews.Core.Api;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder webAppBuilder = GetApplicationBuilder();
WebApplication webApp = webAppBuilder.Build();

/* Instantiate required services */
IApiMapper apiMapper = webApp.Services.GetRequiredService<IApiMapper>();

ConfigureWebApp(webApp, apiMapper);
webApp.Run();

static WebApplicationBuilder GetApplicationBuilder()
{
    WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder();

    /* Register all custom interfaces implementations to make them usable via DI */
    TypeRegistrations.RegisterCustomServices(webAppBuilder);

    webAppBuilder.Services.AddEndpointsApiExplorer();
    webAppBuilder.Services.AddSwaggerGen();
    webAppBuilder.Services.AddMemoryCache();
    webAppBuilder.Services.AddAutoMapper(typeof(AutoMapperProfile));

    return webAppBuilder;
}

static void ConfigureWebApp(WebApplication webApp, IApiMapper apiMapper)
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI();

    /* Register endpoints */
    apiMapper.RegisterEndpoints(webApp);
}

