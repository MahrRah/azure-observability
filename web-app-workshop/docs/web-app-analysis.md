# Web App Analysis

## Available Azure code samples

* [Basic web application - empty web app](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/app-service-web-app/basic-web-app?tabs=cli) ([code](https://github.com/mspnp/samples/tree/master/solutions/basic-web-app))
  * It includes Azure App Service plan and an empty application, Azure SQL Database, Azure Key Vault for storing the database connection string, and Azure Monitor for logging, monitoring, and alerting.
  * Deployed resources send diagnostics logs and metrics to the Log Analytics workspace. So we would have infrastructure monitoring for free.
  * Deployment done through ARM template
* [Scalable web app](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/app-service-web-app/scalable-web-app)
  * Built on top of the Basic web application
  * It includes App Service, Function App, Azure Service Buss Queue, Redis cache, SQL Database, CosmosDB, Cognitive Search, Front Door
  * All three web applications use Application Insights for logs and telemetry. Application Insights will detect SQL Database and Azure Cache for Redis calls as dependencies.
  * Deployment through scripts with az-cli and Visual Studio (for publishing Azure Functions)
* [Serverless web application](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/serverless/web-app)
  * 2 code samples
    * [Drone Delivery Serverless](https://github.com/mspnp/serverless-reference-implementation/tree/v0.1.0-update)
      * 2 architecture there: serverless web app, serverless event processing
      * Deployment through scripts with az-cli
      * Basic monitoring of Azure Functions with Application Insights
    * [ToDo API](https://github.com/Azure-Samples/serverless-web-application)
      * Static website
      * The application serves static Angular.JS content from Azure Blob Storage (Static Website), and implements REST APIs for CRUD of a to do list with Azure Functions. The API reads data from Cosmos DB and returns the results to the web app.
      * Azure Bicep for Infrastructure as Code
      * Basic monitoring of Azure Functions with Application Insights
* [Configure an Azure Web App to call Azure Cache for Redis and Azure SQL Database via Private Endpoints](https://github.com/azure-samples/web-app-redis-sql-db/tree/main/)
  * Focus on the network topology
  * An Application Insights resource used by the Azure Web Apps app to store logs, traces, requests, exceptions, and metrics.
  * Deployment done through ARM template
* [Web App Monitoring](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/app-service-web-app/app-monitoring) - only docs, no reference implementation
* [Best practices for monitoring cloud applications](https://docs.microsoft.com/en-us/azure/architecture/best-practices/monitoring) - only docs, no reference implementation

## Workshop proposal

### Main goal

Using a simple application show users how to implement extended observability. Define basic SLO/A/I and build observability solution around it.

### Application: Recipe aggregator website

The application serves static website, and implements REST APIs for CRUD of recipe operations with App Service. A user can add a new recipe containing recipe name, recipe content or URL to it, and optionally a picture. This information is stored in proper storage service. Website shows last x recipes or a specific recipe when its name is provided.

### Azure services

* App Service
* Application Insights
* Azure Monitor and Log Analytics
* Storage (SQL, CosmosDB, Azure Storage, other)
* Azure Cache for Redis (optional)

### Application monitoring

* Use tools available out of the box - connect Application Insights to the App Service
  * Show metrics, dependencies
* Add custom metric, dependency
* Add logging (structured)
* Add tracing (show distributed tracing between frontend and backend)
* Enrich telemetry data with custom fields
* Add [availability tests](https://docs.microsoft.com/en-us/azure/azure-monitor/app/monitor-web-app-availability)

### Infrastructure monitoring

* Show available platform metrics (for infrastructure monitoring)
* Enable diagnostics
* Check what else could be part of the infrastructure monitoring

### Data analysis - alerts, kusto queries, dashboard

* show kusto query
* create metric, log (stateless and statefull), activity log alerts
* create dashboard (single or more, depending how much we want to show)
* create alerts and dashboard with IaC (Terraform, Bicept, ARM templates)

### Cost monitoring

* Show available panels for cost monitoring
* Create alert for cost monitoring
