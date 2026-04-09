# dotnet-template

**dotnet-template** is an ASP.NET CORE C# template for building traditional HTTPS APIs and MCP (Model Context Protocol) APIs. It follows Clean Architecture and Clean Code best practices by using IoC, DI, and more... Built-in authorization workflow with OAuth JWT tokens or/and MS Entra ID.

## Prerequisites:

- .NET 10 SDK installed.

- [OPTIONAL] If using VSCode IDE install ".NET Install Tool", "C#" and "C# DEV Kit" recommended extensions. Please use Microsoft official extensions. 

## Architecture

**dotnet-template** C# solution is composed of 5 different layers:

- Domain Layer
- Application Layer
- Infrastructure Layer
- API Layer
- MCP Layer

![Architecture Diagram](./docs/images/architecture.png)

> For more information on clean architecture revisit "*docs/clean-architecture.md*" file

The advantages of integrating the MCP layer within the same dotnet project as the conventional API has a major advantage: 

- Application Services are reused, compiled, and bundled into the MCP server artifacts, reducing resource consumption by avoiding network overhead and extra bandwidth from API calls since application services also run within the MCP server. While this increases the bundle size, the impact on backend applications is not specially important.

Moreover, MCP can be configured as well in the same layer as the HTTPS API. This way you can deploy them together saving resources depending on your VPS and Cloud configuration. However, the recommended approach based on "modelcontextprotocol/csharp-sdk" best practices is to separate these APIs into different layers, if you manage accordingly your cloud configuration no extra costs should be incurred.

## Instructions

0. Set your preferred ASPNETCORE environment.

    i.e.: Windows PowerShell
    ```ps
    $env:ASPNETCORE_ENVIRONMENT="Local"
    ```

    i.e.: Windows Cmd
    ```cmd
    set ASPNETCORE_ENVIRONMENT=Local
    ```

    i.e.: Linux Bash
    ```bash
    export ASPNETCORE_ENVIRONMENT=Local
    ```

1. Apply your application settings and MS Entra ID configuration to *appsettings.{env}.json* file. By default *appsettings.json* is used unless you specify and environment.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ApplicationSettings": {
    "AzureAd": {
      "Instance": "https://login.microsoftonline.com/",
      "ClientId": "...",
      "TenantId": "...",
      "Audience": "api://..."
    },
    "ServerUrl": "https://localhost:8443",
    "McpServerUrl": "https://localhost:9443",
    "DatabaseSecretArn": "arn:aws:secretsmanager:region:<account-id>:secret:<secret-name>"
  },
  "AllowedHosts": "*"
}
```

2. Build all projects within the solution

    ```bash
    cd dotnet-template/src
    dotnet build
    ```

3. Execute HTTPS and MCP APIs

    2.1. Run the HTTPS API

    ```bash
    dotnet run --project dotnet-template/src/API/API.csproj
    ```

    2.2. Run the MCP API

    ```bash
    dotnet run --project dotnet-template/src/MCP/MCP.csproj
    ```

## Debugging Instructions

You can debug both HTTPS and MCP APIs using VSCode and Visual Studio 2022 or greater.

- VSCode:
    - Execute VSCode debugger with "*.vscode/launch.json*" configurations. Use "HTTPS API" and/or "MCP API".
    - Execute VSCode debugger with "*Properties/launchSettings.json*" project configurations. You will need to install ".NET Install Tool", "C#" and "C# DEV Kit" VSCode extensions.


- Visual Studio 2022 or greater:
    - Execute Visual Studio debugger using "*Properties/launchSettings.json*" project configurations.

## Emulating MCP calls

1. Run https MCP project

2. Run MCP Inspector

    ```bash
    $env:NODE_TLS_REJECT_UNAUTHORIZED=0
    npx @modelcontextprotocol/inspector
    ```

3. Connect to MCP server through MCP Inspector

    ```bash
    Streamable HTTP: https://localhost:9433/mcp
    ```

- Please specify an **Authorization** header if using JWT and MS Entra ID authorization. 

    ```http
    "Authorization": "Bearer eyJ0eXAiOiJKV1Q..."
    ```

## Connecting MCP to GitHub Copilot Agent

To connect your GitHub Copilot Agent to your MCP tools, please specify the MCP server configuration and its URL within "*.vscode/mcp.json*" file.

\* Take into account the MCP URL, the MCP underlying protocol (HTTP, SSE, Stdio), authorization and any additional header your may use.

```json
{
	"servers": {
		"todos": {
			"url": "https://localhost:9443/mcp",
			"type": "http",
		}
	},
	"inputs": [],
}
```

## Container Instructions

To run the dotnet API within a Docker Container:

1. Build the container image

```console
cd src
```

```console
docker build -t api .
```

2. Run the container image

```console
docker run -p 8443:443 -p 8080:80 api
```

3. Access the image using a web browser https://localhost:8443/scalar/v1 or execute a curl command

```console
curl.exe -k https://localhost:8443/scalar/v1 --verbose
```

## Payment Provider Integration: Stripe

For local development and testing use a Stripe Sandbox:

1. Create Stripe Sandbox: Recurring pricing model > Flat rate > Pre-built checkout form 

2. Create recurring product > Name: Pro Subscription, Currency: EUR, Recurring: Monthly, Price: $9.99

3. Get keypair from sandbox developer section:
    - SecretKey
    - PublishableKey

4. Get Product and Price IDs:
    - ProSubscriptionProductId
    - ProSubscriptionPriceId

5. Install Stripe CLI:

    5.1. Download the latest windows ZIP file from GitHub.

    5.2. Unzip the file stripe_X.X.X_windows_x86_64.zip.

    5.3. Add the path to the unzipped stripe.exe file to your Path environment variable (e.g., C:\Program Files\Stripe).
    
6. Execute Stripe webhook to route the payment your local endpoint:

    ```bash
    stripe listen --forward-to https://localhost:9080/api/payments/webhook
    You have not configured API keys yet. Running `stripe login`...
    Your pairing code is: trump-zenith-openly-yay
    This pairing code verifies your authentication with Stripe.
    Press Enter to open the browser or visit https://dashboard.stripe.com/stripecli/confirm_auth?t=...
    ```

    Confirm it using your account and the link, and copy the WebhookSecret

7. Fill appsettings.*.json file

    ```json
    {
        ...
        "StripeSettings": {
            "SecretKey": "sk_test_51TK37AFSmC4Q2yMBPrQWKf0q0AsuZLFUvkg29mHvdnAeZyZ7QqvyLMEM7Rn5yeh2HNfJeo4mcDTBZgNN1qW70iKw00o0JWhcYL",
            "PublishableKey": "pk_test_51TK37AFSmC4Q2yMBiISLggHIbLfUp2losjogah7cpdrMRsUQtutrAa8d34HC6J8Df7YCcbeINd0HY5P3tQzvdnRW003ZCBQO33",
            "ProSubscriptionProductId": "prod_UIeZdBbYg2z8i5",
            "ProSubscriptionPriceId": "price_1TK3G2FSmC4Q2yMBqWnMy6xF",
            "WebhookSecret": "whsec_535a14a95e126e35a4e6de35618f3e4221a959ae5dfc077fb057d44c0754d990"
        },
        ...
    }
    ```