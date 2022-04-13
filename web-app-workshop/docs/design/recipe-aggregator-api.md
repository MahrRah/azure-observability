# WIP - Recipe aggregator API

## Overview / Problem Statement

Using a simple application show users how to implement extended observability.

This milestone includes creating a Recipe aggregator RESTful API application that later will be extended by workshop participants with Azure observability code.

## Goals / In-Scope

* Recipe aggregator RESTful API is created
  * A user can add a new recipe containing recipe name, recipe content or URL to it
  * A user can upload a picture for a recipe
  * A user can update specified recipe (including updating a picture)
  * A user can delete specified recipe (deleting recipe means also deleting a picture)
  * Recipes can be listed (there is a pagination implemented)
  * Only authorized user (admin) is able to add/delete/modify recipes. Listing or viewing specific recipe is available for everyone.

## Non-goals / Out-of-Scope

* integrating the Recipe API with static website

## Proposed Design / Suggested Approach

Recipe aggregator RESTful API is implemented using ASP.NET Core API and deployed to App Service. Uploaded by user pictures are stored in Azure Blob Storage. Recipe data (recipe name, recipe content or recipe URL, picture URI from Azure Blob Storage) are stored as documents in a CosmosDB. Web App should use Managed Identity instead of connection strings when accessing other Azure components.
Authentication and authorization is implemented according to the [ASP.NET Core security recommendations](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0).

The performance and scalability of the recipe aggregator API is not a concern at this point, thus there is no need to implement caching, however, [a distributed caching](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0) can be added at a later stage.

Deployment process is automated.

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
| Out of the box observability with Application Insights | Harder to add custom instrumentation  |
|                                                        |                                       |

## Technology

* ASP.NET Core API app
* App Service
* CosmosDB
* Azure Blob Storage for images
* Redis Cache or CDN Cache?
* [Managed Identity](https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity)
* Key Vault for storing secrets
* Azure Bicep for Infrastructure as Code

## Non-Functional Requirements

* What are the primary performance and scalability concerns for this milestone/epic?
* Are there specific latency, availability, and RTO/RPO objectives that must be met?
* Are there specific bottlenecks or potential problem areas? For example, are operations CPU or I/O (network, disk) bound?
* How large are the data sets and how fast do they grow?
* What is the expected usage pattern of the service? For example, will there be peaks and valleys of intense concurrent usage?
* Are there specific cost constraints? (e.g. $ per transaction/device/user)

## Operationalization

* Are there any specific considerations for the CI/CD setup of milestone/epic?
* Is there a process (manual or automated) to promote builds from lower environments to higher ones?
* Does this milestone/epic require zero-downtime deployments, and if so, how are they achieved?
* Are there mechanisms in place to rollback a deployment?
* What is the process for monitoring the functionality provided by this milestone/epic?

## Dependencies

* none

## Risks & Mitigations

* Does the team need assistance from subject-matter experts?
* What security and privacy concerns does this milestone/epic have?
* Is all sensitive information and secrets treated in a safe and secure manner?

## Open Questions

* How stable is Azure Bicep? Will we be able to deploy all Azure components, including alerts, dashboards?
* What kind of deployment do we want to have? Should it be a GitHub pipeline? If so, then we will need an Azure subscription. Otherwise, we could just prepare deployment scripts that user could run with their own Azure subscriptions.

## Additional References

* [Managed identity for web app](https://docs.microsoft.com/en-us/azure/app-service/scenario-secure-app-access-storage)
* [Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-6.0)
* [Authentication and authorization in ASP.NET Web API](https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api)