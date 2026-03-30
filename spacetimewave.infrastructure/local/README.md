# spacetimewave.infrastructure

This project is used for **local** deployment of all microservices using docker compose.

## Instructions:

To spin up all the local infraestructure:

1. Install Docker or any Container provider.

2. Run all microservices.

    2.1. Run it with nerdctl using containerd API.

    ```console
    cd spacetimewave.infrastructure/local
    nerdctl compose up
    ```

    2.2. Run it with Docker CLI using dockerd API.

    ```console
    cd spacetimewave.infrastructure/local
    docker-compose up
    ```

> Disclaimer: Change the default passwords as soon as possible and use user secrets when deploying to prod.

3. Access the services:

- Keycloak Admin Dashboard: http://localhost:4001/
- Keycloak Realm: http://localhost:4001/realms/<realm>/account
- Keycloak Database: 
    ```
    host: keycloakdb, port: 5432, db: postgres, user: keycloak, password: password
    ``` 

- pgAdmin Dashboard: http://localhost:5050/
    ```
    User: admin@admin.com
    Password: admin
    ``` 
- API: https://localhost:8443/scalar/v1
- Aspire Dashboard: http://localhost:18888/