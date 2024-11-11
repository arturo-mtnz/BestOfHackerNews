namespace BestOfHackerNews.Core.Services;

using System;
using System.Text.Json;
using BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Convenience implementation of IHttpSimpleClient.
/// </summary>
internal class HttpSimpleClient : IHttpSimpleClient
{
    public HttpSimpleClient(HttpClient httpClient)
    {
        this.HttpClient = httpClient;

        this.JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private HttpClient HttpClient { get; }

    private JsonSerializerOptions JsonSerializerOptions { get; }

    public async Task<T> GetAsync<T>(string uri)
    {
        HttpResponseMessage response = await this.HttpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content, this.JsonSerializerOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize {uri} result into a valid {typeof(T)}.");
    }
}
