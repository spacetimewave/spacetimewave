# Spacetimewave Organization

Spacetimewave is an organization.

## Repository structure

Spacetimewave repository is a monorepo containing all the company's microservices.

The repository folder structure is the following:

spacetimewave
|__ spacetimewave.infrastructure
|   |__dev
|   |__local
|       |__docker-compose.yml
|__ spacetimewave.services
    |__spacetimewave.auth.service
    |__spacetimewave.front.service
    |__spacetimewave.micro.service

## Infrastructure

All the infrastructure microservices are deployed as isolated containers. Use spacetimewave.infrastructure/local/docker-compose.yml file to explore them. Launch them using Rancher and containerd:

```
nerdctl compose up [--build]
```

## Microservices

### Keycloak Identity Provider Service

Keycloak is an identity service used for authentication and authorization purposes, employing OAuth2.0, OpenID Connect (OIDC) and JSON Web Tokens (JWT). The 'spacetimewave' realm is used as the main tenant for all microservices.

To deploy Keycloak alongside all microservices use (default):

```
spacetimewave.infrastructure > local > docker-compose.yml
```

To deploy Keycloak as an isolated service and save the realm configuration: 

```
spacetimewave.services > spacetimewave.auth.service > docker-compose.yml
```

There is another container running the keycloak PostgreSQL database. 

### pgAdmin Service

Service used to connect to any PostgreSQL database.

### Micro Service

Dotnet API microservice.

### Frontend Service

Dotnet API microservice.

### OTEL Service

Open Telemetry Service using Aspire to monitor the company microservices.