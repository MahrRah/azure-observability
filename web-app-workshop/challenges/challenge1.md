# Challenge 1 - Identify and improve Recipe Aggregator API response time problem

## Context - TBC

Recipe Aggregator API is slow. You need to find a root cause and fix it.
Explain concept of SLI/O and introduce here the first SLO:
response time of the Recipe API is below 3 seconds for 99,9% of requests.

## Tips

1. Observe current Recipe API state
Before improving the Recipe API code, we need to know what is the current performance and what response times are returned. Review if there are errors or there are only some parts of the API that are slow. You can easily enable telemetry collection by adding Application Insights service to the WebApp builder in `Program.cs` (there is already a code for it, just uncomment one line, required NuGet package is already added) and specify an Application Insights connection string in `appsettings.json`.
2. Enable verbose logging
Logs may provide very useful details. Recipe API has some logs defined (in `RecipesController.cs`) but there are also logs provided by the AspNetCore or EntityFramework. However, not all of messages may be logged. Consider changing [logging levels in logging configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0#configure-logging) to enable more detailed logging for the time of debugging and troubleshooting either for Console or ApplicationInsights logger provider. Also make sure that ApplicationInsights logging provider captures log levels that you want (there may be explicit log level override required, see [more details](https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core#how-do-i-customize-ilogger-logs-collection)).
