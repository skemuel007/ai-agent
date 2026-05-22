# Continuation Notes

## Current State

The repo contains two sample sections:

- `section4-setup`
- `section5-getting-started`

Both depend on:

- Root `.env` configuration.
- `shared/Shared/EnvironmentHelper.cs`.
- Central package versions in `Directory.Packages.props`.

The latest expected validation command is:

```bash
dotnet build ai-agents.slnx
```

## Existing Branch Context

Work has been pushed to:

- `main`
- `section5-getting-started`

Check the current branch before continuing:

```bash
git status --short --branch
```

## Important Design Decisions

Environment loading belongs in the shared class library, not in each console app.

New sample apps should call:

```csharp
EnvironmentHelper.LoadRootEnv();
```

Then read required variables with:

```csharp
EnvironmentHelper.GetRequiredEnvironmentVariable("VARIABLE_NAME");
```

Package versions belong in `Directory.Packages.props`.

Each section should have its own README with:

- Purpose.
- Project layout.
- Prerequisites.
- Configuration.
- Run commands.
- Package explanation.
- How the app works.

## Likely Next Work

Future sections will probably add more agent patterns. Keep reusable setup code in `shared/Shared` and keep each section focused on the lesson-specific code.

If adding a new section, copy the structure of `section5-getting-started` rather than duplicating setup logic.
