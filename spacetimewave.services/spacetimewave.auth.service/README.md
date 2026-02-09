# spacetimewave.auth.service

This service is used for **authentication** and **authorization** with JWT tokens, OAuth 2.0, OIDC, SAML, and MFA.

## Instructions:

To spin up Keycloak with Postgres please follow the following steps:

1.  Install Docker or any Container provider.

2. Run Keycloak service, Postgres and pgAdmin containers.

    ```console
    docker-compose up
    ```

3. Access Keycloak using the web portal (http://localhost:8080) and the following credentials:

    ```
    > username: admin
    > password: admin
    ```

4. Access pgAdmin using the web portal (http://localhost:5050) and the following credentials:

    ```
    > username: admin@admin.com
    > password: admin
    ```

5. After logging into pgAdmin, register the Keycloak Postgres database:

    ```
    > hostname: keycloakdb
    > port: 5432
    > maintenance database: postgres
    > user: keycloak
    > password: password
    ```

Now, you are ready to start configuring your authentication and authorization servers with Keycloak.

> Disclaimer: Change the default passwords as soon as possible and use user secrets when deploying to prod.

## Realm and Client Configurations:

1. Access Keycloak Admin Portal using the following URL http://localhost:8080/.

2. Create a Realm.

3. Activate the Realm.

4. Configure your Realm settings. 
4.1. Allow user registration

5. Register a user or log in to the Realm using the URL http://localhost:8080/realms/<realm>/account.

E.g.: http://localhost:8080/realms/spacetimewave/account

6. After logging in, you can configure your settings within the Realm.