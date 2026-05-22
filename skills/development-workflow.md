# Development Workflow

## Build

Build the full solution:

```bash
dotnet build ai-agents.slnx
```

Build section 4 only:

```bash
dotnet build section4-setup/HelloAgent/HelloAgent.csproj
```

Build section 5 only:

```bash
dotnet build section5-getting-started/BasicAgentApp/BasicAgentApp.csproj
```

## Run

Run section 4:

```bash
dotnet run --project section4-setup/HelloAgent/HelloAgent.csproj
```

Run section 5:

```bash
dotnet run --project section5-getting-started/BasicAgentApp/BasicAgentApp.csproj
```

## Adding A New Section

Use this pattern for future sections:

1. Create a folder named `sectionN-topic-name`.
2. Add a console app under that folder.
3. Reference `../../shared/Shared/Shared.csproj`.
4. Add the project to `ai-agents.slnx`.
5. Use central package versions from `Directory.Packages.props`.
6. Add a section-specific `README.md`.
7. Build the full solution.

## Package Management

This repo uses central package management.

Add versions to `Directory.Packages.props`:

```xml
<PackageVersion Include="Package.Name" Version="x.y.z" />
```

Reference packages in project files without versions:

```xml
<PackageReference Include="Package.Name" />
```

## Git Notes

Current remote:

```text
origin https://github.com/skemuel007/ai-agent.git
```

Before committing:

```bash
git status --short --branch
dotnet build ai-agents.slnx
```

Do not commit:

- `.env`
- `bin/`
- `obj/`
- IDE user settings
- secrets or real Azure keys
