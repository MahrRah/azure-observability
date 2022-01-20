# Web App Analysis - what is available?

* [Empty WebApp](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/app-service-web-app/basic-web-app?tabs=cli) ([code](https://github.com/mspnp/samples/tree/master/solutions/basic-web-app)) but with full architecture deployment (App Service, KeyValut, SQL instance, Log Analytics).
  * Deployed resources send diagnostics logs and metrics to the Log Analytics workspace. So we would have infrastructure monitoring for free.
  * Deployment done through ARM template
* [Serverless web application](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/serverless/web-app)
  * 2 code samples
    * [Drone Delivery Serverless](https://github.com/mspnp/serverless-reference-implementation/tree/v0.1.0-update)
      * 2 architecture there: serverless web app, serverless event processing
      * Deployment through scripts with az-cli
      * Basic monitoring of Azure Functions with Application Insights
  * [ToDo API](https://github.com/Azure-Samples/serverless-web-application)
    * Static website
    * Azure Bicep for Infrastructure as Code
    * Basic monitoring of Azure Functions with Application Insights
* [Scalable web app](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/app-service-web-app/scalable-web-app)
  * All three web applications use Application Insights for logs and telemetry. That data can be view from the Azure portal. Application Insights will detect SQL Database and Azure Cache for Redis calls as dependencies.
  * Deployment through scripts with az-cli and Visual Studio (for publishing Azure Functions)
* [Configure an Azure Web App to call Azure Cache for Redis and Azure SQL Database via Private Endpoints](https://github.com/azure-samples/web-app-redis-sql-db/tree/main/)
  * An Application Insights resource used by the Azure Web Apps app to store logs, traces, requests, exceptions, and metrics. For more information
  * Deployment done through ARM template
* [Web App Monitoring](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/app-service-web-app/app-monitoring), only docs, no reference implementation
