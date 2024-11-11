*** HOW TO RUN ***

The application can be compiled and run from Visual Studio. Startup project should be set to BestOfHackerNews.Core. After launching the program, the easiest way to use it is via Swagger:

http://localhost:5000/swagger/index.html

From there, the /api/topStories endpoint can be invoked with HTTP GET. The endpoint can also be directly called from a web browser. For example, for obtaining the top 10 stories:

http://localhost:5000/api/topStories?count=10


*** MADE ASSUMPTIONS ***

- It is assumed that it is acceptable to cache the results obtained from the Hacker News API to avoid overloading it with requests, even though we get results outdated by some seconds. 


*** FURTHER POSSIBLE ENHANCEMENTS: ***

- Explicit throttling in HttpClient: it may be convenient to add throttling capabilities to the HttpSimpleClient in order to avoid making more than certain amount of requests per second. Nevertheless, this is mildly accomplished by current design thanks to the caching and the random expiration times for cached stories. 
- Error handling refinement: it may be convenient to return specific HTTP error codes and messages, depending on the API intended usage.
- Disabling HttpClient resilience logging in console: after enabling HttpClient standard resilience policy to allow HttpClient retries, a very verbose console logging for HttpCiente requests has also been activated. I noticed this amount of logging penalizes performance but, still, decided to keep it to allow better observability about which HTTP requests are made to the Hacker News API. However, disabling this verbose logging would noticeably improve performance.
