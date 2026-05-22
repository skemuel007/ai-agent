# Repository Map

## Solution

The solution file is `ai-agents.slnx`.

Current projects:

- `shared/Shared/Shared.csproj`
- `section4-setup/HelloAgent/HelloAgent.csproj`
- `section5-getting-started/BasicAgentApp/BasicAgentApp.csproj`

## Shared Library

`shared/Shared` contains reusable code used by sample apps.

Important file:

- `EnvironmentHelper.cs`

Responsibilities:

- Load the root `.env` file by walking upward from `AppContext.BaseDirectory`.
- Read required environment variables.
- Throw a clear exception when a required variable is missing.

Use this helper in new sample apps instead of duplicating environment-loading code.

## Section 4

`section4-setup/HelloAgent` demonstrates the basic Azure OpenAI agent setup.

It:

- Loads root environment settings through `EnvironmentHelper`.
- Reads `AZURE_OPENAI_ENDPOINT`.
- Reads `AZURE_OPENAI_DEPLOYMENT_NAME`.
- Creates an `AzureOpenAIClient`.
- Converts a chat client to an `AIAgent`.
- Runs a simple prompt.

## Section 5

`section5-getting-started/BasicAgentApp` demonstrates a more explicit agent shape.

It:

- Loads root environment settings through `EnvironmentHelper`.
- Creates an Azure OpenAI chat client.
- Converts it to `IChatClient`.
- Creates a named `AIAgent` called `NetworkSupport`.
- Adds role-specific instructions.
- Sends a VPN/DNS support scenario and prints the agent response.

## Central Packages

Package versions are centralized in `Directory.Packages.props`.

When adding a package:

1. Add the version to `Directory.Packages.props`.
2. Add a versionless `PackageReference` to the project `.csproj`.

Keep common package versions in one place so future sections can reuse them.
