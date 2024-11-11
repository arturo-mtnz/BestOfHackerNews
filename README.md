*** HOW TO RUN ***

The application can be compiled and run from Visual Studio. Startup project should be set to BestOfHackerNews.Core. After launching the program, the easiest way to use it is via Swagger:

http://localhost:5000/swagger/index.html

From there, the /api/topStories endpoint can be invoked with HTTP GET. The endpoint can also be directly called from a web browser. For example, for obtaining the top 10 stories:

http://localhost:5000/api/topStories?count=10


*** MADE ASSUMPTIONS ***

- It is assumed that it is acceptable to cache the results obtained from the Hacker News API to avoid overloading it with requests, even though we get results outdated by some seconds. 
- It is assumed that a maximum number of concurrent outgoing HTTP requests to Hacker news API should be 50.


*** FURTHER POSSIBLE ENHANCEMENTS: ***

- Error handling refinement: it may be convenient to return specific HTTP error codes and messages, depending on the API intended usage.
- HttpClient resilience and throttling capabilities are testable. Adding specific tests would be valuable.
- Minimum severity console logging for Polly and BestOfHackerNews.Core could be increased up to Warning level to improve performance. So far, only System.Net.Http and Microsoft.Extensions.Http.Resilience log origins are filtered to only show Warning or higher log severity.
