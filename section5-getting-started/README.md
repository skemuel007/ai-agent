# Section 5 Getting Started: Basic Agent App

This section introduces a basic AI agent built with the Microsoft Agents AI abstractions and an Azure OpenAI chat deployment.

## Project Layout

- `BasicAgentApp` - console app sample.
- `BasicAgentApp/Program.cs` - creates an Azure OpenAI chat client, converts it to an `IChatClient`, wraps it as an `AIAgent`, and runs a support-agent prompt.
- `BasicAgentApp/BasicAgentApp.csproj` - project file for the sample app.
- `../shared/Shared` - shared class library for reusable helpers.
- `../shared/Shared/EnvironmentHelper.cs` - loads the root `.env` file and reads required environment variables.
- `../Directory.Packages.props` - central package version file shared by projects in this solution.
- `../.env` - local environment file loaded by the app at startup.
- `../.env.example` - safe template for the root `.env` file.

## Prerequisites

- .NET SDK that supports `net10.0`.
- Azure CLI installed and signed in.
- Access to an Azure OpenAI resource with a deployed chat model.

Sign in to Azure:

```bash
az login
```

If needed, select the correct subscription:

```bash
az account set --subscription "<subscription-id-or-name>"
```

## Configuration

The app loads configuration from the root `.env` file using the shared `EnvironmentHelper`.

From the repository root, update `.env`:

```bash
AZURE_OPENAI_ENDPOINT=https://<your-resource-name>.openai.azure.com/
AZURE_OPENAI_DEPLOYMENT_NAME=<your-model-deployment-name>
```

`AZURE_OPENAI_ENDPOINT` is the endpoint for your Azure OpenAI resource.

`AZURE_OPENAI_DEPLOYMENT_NAME` is the deployment name configured in Azure AI Foundry or the Azure portal.

The app authenticates with:

```csharp
new AzureCliCredential()
```

That means it uses the identity from your active Azure CLI session.

## Run the Sample

From this `section5-getting-started` folder, restore packages:

```bash
dotnet restore BasicAgentApp/BasicAgentApp.csproj
```

Build the project:

```bash
dotnet build BasicAgentApp/BasicAgentApp.csproj
```

Run the console app:

```bash
dotnet run --project BasicAgentApp/BasicAgentApp.csproj
```

The app sends a VPN/DNS support issue to an agent named `NetworkSupport` and prints the response.

## Installed Packages

Package versions are managed centrally in `../Directory.Packages.props`.

The `BasicAgentApp` project references:

### `Azure.AI.OpenAI`

Provides the Azure OpenAI client SDK. The sample uses `AzureOpenAIClient` to connect to the Azure OpenAI endpoint and create a chat client for the configured deployment.

### `Azure.Identity`

Provides Azure authentication credentials. The sample uses `AzureCliCredential`.

### `Microsoft.Agents.AI.OpenAI`

Provides integration between OpenAI chat clients and Microsoft Agents AI. The sample uses `.AsIChatClient()` and `.AsAIAgent(...)`.

## How The App Works

The app loads the root `.env` file:

```csharp
EnvironmentHelper.LoadRootEnv();
```

It reads the Azure OpenAI settings:

```csharp
var endpoint = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
var deploymentName = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME");
```

It creates an `IChatClient`:

```csharp
IChatClient chatClient = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(deploymentName)
    .AsIChatClient();
```

Then it creates a named agent with instructions:

```csharp
AIAgent supportAgent = chatClient.AsAIAgent(
    name: "NetworkSupport",
    instructions: "You are a Tier 1 IT Support Agent. Your answers should be concise, professional, and limited strictly to the topic at hand, do not provide answer outside your role.");
```
