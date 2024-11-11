namespace BestOfHackerNews.Core.Services.Contracts;

/// <summary>
/// Convenience HttpClient wrapper
/// </summary>
internal interface IHttpSimpleClient
{
    /// <summary>
    /// Asynchronously gets the result of the given remote endpoint and deserializes it to type T.
    /// </summary>
    /// <typeparam name="T">The returned type</typeparam>
    /// <param name="uri">The requested URI</param>
    /// <returns>The deserialized value of tyoe T returned by the endpoint</returns>
    Task<T> GetAsync<T>(string uri);
}
