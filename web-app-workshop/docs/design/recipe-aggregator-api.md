# Recipe aggregator API

## Overview / Problem Statement

Using a simple application show users how to implement extended observability.

This milestone includes creating a Recipe aggregator RESTful API application that later will be extended by workshop participants with Azure observability code.

## Goals / In-Scope

* Recipe aggregator RESTful API is created
  * A user can add a new recipe containing recipe name, recipe content or URL to it
  * A user can update specified recipe
  * A user can delete specified recipe
  * Recipes can be listed (there is a pagination implemented)
  * Only authorized user (admin) is able to add/delete/modify recipes. Listing or viewing specific recipe is available for everyone.

## Non-goals / Out-of-Scope

* integrating the Recipe API with static website

## Proposed Design / Suggested Approach

Recipe aggregator RESTful API is implemented using ASP.NET Core API and deployed to App Service. Recipe data (recipe name, recipe content or recipe URL) are stored as documents in a CosmosDB. Web App should use Managed Identity instead of connection strings when accessing other Azure components.
Authentication and authorization is implemented according to the [ASP.NET Core security recommendations](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0).

The performance and scalability of the recipe aggregator API is not a concern at this point, thus there is no need to implement caching, however, [a distributed caching](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0) can be added at a later stage.

Deployment process of Azure resources is automated with [Bicept](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/). As part of this milestone there is no need to add a CI/CD deployment pipeline. We will provide users with a simple script or documentation steps explaining how to deploy the solution to their own Azure subscriptions.

| Pros                                                                           | Cons                                  |
| ------------------------------------------------------------------------------ | ------------------------------------- |
| Simple to implement RESTful API in ASP.NET Core                                | There are other Web App code samples  |
| Available out-of-the-box observability with Application Insights               |                                       |
| ASP.NET Core Web App can be easily extended with custom instrumentation code   |                                       |
| Multiple Azure components involved to show different observability aspects     |                                       |
|                                                                                |                                       |

Alternative approach: using Serverless Function App instead of App Service
| Pros                                                   | Cons                                  |
| ------------------------------------------------------ | ------------------------------------- |
| Out-of-the-box observability with Application Insights | Harder to add custom instrumentation  |
|                                                        |                                       |

## Technology

* ASP.NET Core API app
* App Service
* CosmosDB
* [Managed Identity](https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity)
* Key Vault for storing secrets
* Azure Bicep for Infrastructure as Code

## Non-Functional Requirements

* What are the primary performance and scalability concerns for this milestone/epic?
  * SLOs:
    * the Recipe API availability is greater than or equal to 99,9% (exact [availability test]((https://docs.microsoft.com/en-us/azure/azure-monitor/app/availability-overview)) will be defined later),
    * response time of the Recipe API is below 5 seconds for 99,9% of requests.
* Are there specific latency, availability, and RTO/RPO objectives that must be met?
  * No
* Are there specific bottlenecks or potential problem areas? For example, are operations CPU or I/O (network, disk) bound?
  * Incorrectly designed database schema, so multiple CosmosDB calls are required to retrieve data.
  * Fetching/returning too large amount of data at once
    * a user can put large text data, so some limits should be defined for maximum size of recipes,
    * number of returned recipes in a paginated response should be properly defined.
  * Using only a single Azure region for CosmosDB or deploying Azure App Service and CosmosDB in different Azure regions can cause latency.
* How large are the data sets and how fast do they grow?
  * The Recipe API will be used for learning purposes, so the growth probably will not be big. But there should be defined some limits for the data size as mentioned previously.
* What is the expected usage pattern of the service? For example, will there be peaks and valleys of intense concurrent usage?
  * As mentioned before the recipe API will be used for learning purposes, so the usage will be minimal.
* Are there specific cost constraints? (e.g. $ per transaction/device/user)
  * There is no specific constrains, but it will be a learning sample, so the pricing tiers for all Azure resources should be chosen wisely to not generate high cost.

## Operationalization

* Are there any specific considerations for the CI/CD setup of milestone/epic?
  * For this milestone we can create a basic pipeline for building, checking format and running unit tests for the solution. There is no need to create a deployment pipeline.
* Is there a process (manual or automated) to promote builds from lower environments to higher ones?
  * Non-applicable
* Does this milestone/epic require zero-downtime deployments, and if so, how are they achieved?
  * Non-applicable
* Are there mechanisms in place to rollback a deployment?
  * Non-applicable
* What is the process for monitoring the functionality provided by this milestone/epic?
  * Monitoring will be tackled in next milestones.

## Dependencies

* none

## Risks & Mitigations

* Does the team need assistance from subject-matter experts?
  * No
* What security and privacy concerns does this milestone/epic have?
  * No
* Is all sensitive information and secrets treated in a safe and secure manner?
  * The solution will use Managed Identity and all secrets should be stored in Key Vault.

## Additional References

* [Managed identity for web app](https://docs.microsoft.com/en-us/azure/app-service/scenario-secure-app-access-storage)
* [Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-6.0)
* [Authentication and authorization in ASP.NET Web API](https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api)