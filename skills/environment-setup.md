# Environment Setup

## Root `.env`

The repository uses a root `.env` file for local configuration.

Expected location:

```text
.env
```

Template:

```text
.env.example
```

Required variables:

```bash
AZURE_OPENAI_ENDPOINT=https://<your-resource-name>.openai.azure.com/
AZURE_OPENAI_DEPLOYMENT_NAME=<your-model-deployment-name>
```

Optional variable for API key authentication:

```bash
AZURE_OPENAI_KEY=<your-azure-openai-key>
```

The real `.env` file is ignored by Git. Do not commit secrets.

## Loading Environment Variables

.NET console apps do not automatically load `.env` files.

This repo uses `DotNetEnv` inside `shared/Shared/EnvironmentHelper.cs`:

```csharp
EnvironmentHelper.LoadRootEnv();
```

The helper walks upward from the app build output directory until it finds `.env`.

Use this in new console apps before reading environment variables:

```csharp
EnvironmentHelper.LoadRootEnv();

var endpoint = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
var deploymentName = EnvironmentHelper.GetRequiredEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME");
```

## Azure Authentication

The current samples use Azure CLI authentication:

```csharp
new AzureCliCredential()
```

Before running a sample:

```bash
az login
```

If needed:

```bash
az account set --subscription "<subscription-id-or-name>"
```

## Rider Notes

Rider may hide `.env` because it is a dotfile and ignored by Git.

To view it:

- Enable hidden files in the Project tool window.
- Switch from Solution view to File System view if needed.

The app does not require Rider run configuration environment variables because the code loads the root `.env` itself.
