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

## Non-goals / Out-of-Scope

* integrating the Recipe API with static website

## Proposed Design / Suggested Approach

Recipe aggregator RESTful API is implemented using ASP.NET Core API and deployed to App Service. Uploaded by user pictures are stored in Azure Blob Storage. Recipe data (recipe name, recipe content or recipe URL, picture URI from Azure Blob Storage) are stored as documents in a CosmosDB. Web App should use Managed Identity instead of connection strings when accessing other Azure components.

Deployment process is automated.

| Pros                                                                           | Cons                                  |
| ------------------------------------------------------------------------------ | ------------------------------------- |
| Simple to implement RESTful API in ASP.NET Core                                | There are other Web App code samples  |
| Available out of the box observability                                         |                                       |
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

* Authentication/authorization - only authorized user should be able to modify recipes. But viewing could be available for everyone. Where should this be handled? In the API? Other component?
* How stable is Azure Bicep? Will we be able to deploy all Azure components, including alerts, dashboards?
* What (only pictures, or recipe content, both) and how should we cache (Redis Cache, CDN cache, other?) Or we don't need it?

## Additional References

* [Managed identity for web app](https://docs.microsoft.com/en-us/azure/app-service/scenario-secure-app-access-storage)