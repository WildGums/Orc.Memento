# Orc.Memento

Orc.Memento is a library that adds support for undo/redo functionality in .NET applications. It is built on top of Catel and implements the Memento design pattern, allowing applications to track and revert changes to objects and collections.

The library consists of the following projects:

- `Orc.Memento` — Core library with undo/redo support, including the `MementoService`, action types, and observers.
- `Orc.Memento.Tests` — Unit tests for all library functionality.
- `Orc.Memento.Example` — Example application demonstrating library usage.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 2. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 3. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.Memento | `master` |
| Orc.Memento | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Overview

```
Orc.Memento          => Core undo/redo library (cross-platform .NET)
Orc.Memento.Tests    => NUnit test project
Orc.Memento.Example  => Example application
```

### Key Concepts

| Concept | Description |
|---------|-------------|
| `IMementoService` / `MementoService` | Central service for tracking undo/redo operations |
| `IMementoBatch` / `Batch` | Groups multiple undo operations into a single undoable unit |
| `UndoBase` | Base class for all undo actions |
| `ActionUndo` | Undo action backed by arbitrary delegates |
| `PropertyChangeUndo` | Undo action that reverts a property change |
| `CollectionChangeUndo` | Undo action that reverts a collection change |
| `ObjectObserver` | Observes property changes on a registered object |
| `CollectionObserver` | Observes collection changes on a registered collection |

### Directory Guide

| Directory | Editable? | Notes |
|-----------|-----------|-------|
| `src/Orc.Memento/` | Yes | Core library source |
| `src/Orc.Memento/Services/` | Yes | `MementoService` implementation |
| `src/Orc.Memento/Actions/` | Yes | Undo/redo action types |
| `src/Orc.Memento/Observers/` | Yes | Property and collection observers |
| `src/Orc.Memento.Tests/` | Yes | Unit tests |
| `src/Orc.Memento.Example/` | Yes | Example application |
| `deployment/` | No | Deployment / build scripts |

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Create a Facts class for each feature/class under test (e.g., `MementoServiceFacts`)
3. Nest test methods inside an inner class named after the method under test (e.g., `TheUndoMethod`)
4. Combine Pascal / Snake case for test methods (e.g., `Feature_Does_Work`)

```csharp
[TestFixture]
public class TheUndoMethod
{
    [TestCase]
    public void Undo_Does_Work()
    {
        var mementoService = new MementoService();
        // ... arrange, act, assert
    }
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Documentation portal | https://opensource.wildgums.com |
