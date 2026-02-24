# GitHub Copilot Instructions for LegoStore

## Project Overview

LegoStore is a BrickLink store automation tool that manages physical LEGO inventory.
It automates updating storage state after picking orders processed in BrickStore.

## Technology Stack

- **Language**: C# (.NET)
- **Architecture**: Domain-driven design
- **Testing**: xUnit

## Project Structure

```
legostore/
└── backend/
    ├── domain/       # Core domain models
    ├── services/     # Application services and interfaces
    └── tests/        # Unit tests (xUnit)
```

## Domain Model Hierarchy

```
StoreStorage → Cabinets → CaseGroups → Cases → Containers → Sections
```

| Model        | Description                                              |
|--------------|----------------------------------------------------------|
| `Section`    | Smallest unit; holds one LotId + Quantity                |
| `Container`  | Holds sections; type determines section count            |
| `Case`       | Holds containers of one type; capacity depends on type   |
| `CaseGroup`  | 3×3 grid = 9 Cases                                       |
| `Cabinet`    | Contains 4 CaseGroups                                    |
| `StoreStorage` | N Cabinets; configurable for shop size                 |

## Container Types

| Type  | Sections per Container | Containers per Case |
|-------|------------------------|---------------------|
| PX12  | 3                      | 12                  |
| PX6   | 1                      | 6                   |
| PX4   | 1                      | 4                   |
| PX2   | 1                      | 2                   |

## Coding Guidelines

- Follow C# naming conventions (PascalCase for public members, camelCase for locals).
- Use `IReadOnlyList<T>` for collections exposed via properties to maintain encapsulation.
- Validate arguments in constructors and methods; throw descriptive exceptions.
- Write xUnit tests for all new domain logic and service methods.
- Keep domain models free of infrastructure concerns (no I/O, no DI).
- Use interfaces (`IStorageService`, `IBsxParserService`) to allow future dependency injection.

## Prompt Logging Requirement

**All prompts used during development with GitHub Copilot must be logged.**

For every Copilot interaction during development, append an entry to `docs/copilot-prompts.md` in the following format:

```markdown
## [YYYY-MM-DD] <Short title>

**Prompt:**
> <exact prompt text>

**Context:** <file or feature the prompt was used for>

**Outcome:** <brief description of what was generated or changed>
```

This log is required for traceability and future review of AI-assisted development decisions.
