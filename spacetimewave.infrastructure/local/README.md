# spacetimewave.infrastructure

This project is used for **local** deployment of all microservices using docker compose.

## Instructions:

To spin up all the local infraestructure:

1. Install Docker or any Container provider.

2. Run all microservices.

    ```console
    cd spacetimewave/local
    docker-compose up
    ```

> Disclaimer: Change the default passwords as soon as possible and use user secrets when deploying to prod.