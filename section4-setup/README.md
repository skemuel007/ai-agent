# Section 4 Setup: Hello Agent

This folder contains a small .NET console app that creates and runs an AI agent backed by an Azure OpenAI chat deployment.

## Project Layout

- `HelloAgent` - console app sample.
- `HelloAgent/Program.cs` - reads Azure OpenAI settings from environment variables, creates an `AIAgent`, sends a prompt, and prints the response.
- `HelloAgent/HelloAgent.csproj` - project file with the installed NuGet packages.
- `../shared/Shared` - shared class library for reusable helpers.
- `../shared/Shared/EnvironmentHelper.cs` - loads the root `.env` file and reads required environment variables.
- `../Directory.Packages.props` - central package version file shared by projects in this solution.
- `../.env` - local environment file loaded by the app at startup.
- `../.env.example` - safe template for the root `.env` file.

## Prerequisites

- .NET SDK that supports `net10.0`.
- Azure CLI installed and signed in, unless you switch the sample to API key authentication.
- Access to an Azure OpenAI resource with a deployed chat model.

Sign in to Azure before running the app:

```bash
az login
```

If you have access to multiple tenants or subscriptions, select the correct subscription:

```bash
az account set --subscription "<subscription-id-or-name>"
```

## Configuration

The app reads Azure OpenAI configuration from environment variables.

.NET console apps do not load `.env` files automatically. The shared class library uses `DotNetEnv` and exposes `EnvironmentHelper.LoadRootEnv()` so apps can load the solution-level `.env` file from the repository root.

The `.env` file is ignored by Git because it can contain secrets. Use `../.env.example` as the committed template.

### Default: Azure CLI Authentication

From the repository root, update `.env`:

```bash
AZURE_OPENAI_ENDPOINT=https://<your-resource-name>.openai.azure.com/
AZURE_OPENAI_DEPLOYMENT_NAME=<your-model-deployment-name>
```

`AZURE_OPENAI_ENDPOINT` is the endpoint for your Azure OpenAI resource.

`AZURE_OPENAI_DEPLOYMENT_NAME` is the deployment name you configured in Azure AI Foundry or the Azure portal. This is not always the same as the base model name. For example, your deployment might be named `gpt-4.1-mini`, `chat`, or `my-agent-model`.

The current code uses `AzureCliCredential` by default:

```csharp
new AzureCliCredential()
```

That means the app uses the identity from your active Azure CLI session.

Because the app loads the root `.env`, you do not have to export these values manually. You can still override them from the shell if needed:

```bash
export AZURE_OPENAI_ENDPOINT="https://<another-resource-name>.openai.azure.com/"
export AZURE_OPENAI_DEPLOYMENT_NAME="<another-model-deployment-name>"
```

The root `.env` file is found by walking up from the app's build output directory, so it works when you run the app from the repository root, from `section4-setup`, or directly from the project folder.

If you still see `AZURE_OPENAI_ENDPOINT is not set`, check these items:

- The root `.env` file does not exist.
- The `.env` file is not in the repository root next to `ai-agents.slnx`.
- The variable name has a typo.
- The `.env` line has spaces around `=`, for example `AZURE_OPENAI_ENDPOINT = ...`, which is invalid.
- The app is running from a copied build output outside the repository, so it cannot walk up to the root `.env`.

### Creating The Root `.env`

If the root `.env` file is missing, create it from the example:

```bash
cp ../.env.example ../.env
```

Then edit `../.env` with your real Azure OpenAI values.

### Optional: API Key Authentication

`Program.cs` also includes a branch for API key authentication:

```csharp
var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY")
    ?? throw new Exception("AZURE_OPENAI_KEY is not set");
```

To use that path, set `isLegacySystem` to `true` in `HelloAgent/Program.cs` and add the key to the root `.env`:

```bash
AZURE_OPENAI_KEY=<your-azure-openai-key>
```

Prefer Azure CLI or managed identity authentication for normal development and deployment. Use API keys only when you need compatibility with a legacy setup.

## Run the Sample

From this `section4-setup` folder, restore packages:

```bash
dotnet restore HelloAgent/HelloAgent.csproj
```

Build the project:

```bash
dotnet build HelloAgent/HelloAgent.csproj
```

Run the console app:

```bash
dotnet run --project HelloAgent/HelloAgent.csproj
```

The app currently sends this prompt:

```text
What is the largest city in Nigeria?
```

The response is written to the console.

## Installed Packages

Package versions are managed centrally in `../Directory.Packages.props`. This lets new projects reuse the same package versions without repeating version numbers in every `.csproj`.

The `HelloAgent` project references these NuGet packages:

### `Azure.AI.OpenAI` `2.9.0-beta.1`

Provides the Azure OpenAI client SDK. The sample uses `AzureOpenAIClient` to connect to the Azure OpenAI endpoint and create a chat client for the configured deployment.

### `Azure.Identity` `1.21.0`

Provides Azure authentication credentials. The sample uses `AzureCliCredential` in the default path, which means it authenticates with the identity from your current `az login` session.

### `DotNetEnv` `3.1.1`

Referenced by the shared class library. It loads the root `.env` file into process environment variables before the Azure OpenAI settings are read.

### `Microsoft.Agents.AI.OpenAI` `1.6.2`

Provides integration between OpenAI chat clients and the Microsoft Agents AI abstractions. The sample uses `.AsAIAgent()` to wrap the Azure OpenAI chat client as an `AIAgent`, then calls `RunAsync(...)` to send a prompt.

## How The App Is Configured

At startup, `HelloAgent/Program.cs` uses the shared helper to load the root `.env` file:

```csharp
EnvironmentHelper.LoadRootEnv();
```

`HelloAgent/Program.cs` loads configuration from the environment:

```csharp
var endpoint = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
var model = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME");
```

It then creates the agent:

```csharp
AIAgent agent = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureCliCredential())
    .GetChatClient(model)
    .AsAIAgent();
```

For API key authentication, the code uses `ApiKeyCredential` from `System.ClientModel`:

```csharp
new ApiKeyCredential(apiKey)
```

Finally, the agent runs a prompt and prints the result:

```csharp
Console.WriteLine(await agent.RunAsync("What is the largest city in Nigeria?"));
```
