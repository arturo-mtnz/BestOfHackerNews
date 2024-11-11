namespace BestOfHackerNews.Core.Services;

using System;
using System.Text.Json;
using BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Convenience implementation of IHttpSimpleClient with throttling capabilities to ensure no more than a given number of concurrent connections.
/// </summary>
internal class HttpThrottledClient : IHttpSimpleClient
{
    public HttpThrottledClient(HttpClient httpClient)
    {
        const int maxConcurrentConnections = 50;
        this.ThrottleSemaphore = new SemaphoreSlim(maxConcurrentConnections);

        this.HttpClient = httpClient;

        this.JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private HttpClient HttpClient { get; }

    private JsonSerializerOptions JsonSerializerOptions { get; }

    private SemaphoreSlim ThrottleSemaphore { get; }

    public async Task<T> GetAsync<T>(string uri)
    {
        try
        {
            await this.ThrottleSemaphore.WaitAsync();
            HttpResponseMessage response = await this.HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content, this.JsonSerializerOptions)
                ?? throw new InvalidOperationException($"Failed to deserialize {uri} result into a valid {typeof(T)}.");
        }
        finally
        {
            this.ThrottleSemaphore.Release();
        }
    }
}
