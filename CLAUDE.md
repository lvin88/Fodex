# CLAUDE.md — Fodex Architectural Reference

> **For Claude Code (CLI) and any AI assistant working on this project.**
> Read this file at the start of every session. It encodes the architectural
> decisions, naming conventions, and forbidden patterns that the codebase
> depends on. Violating these rules will fail the architecture tests and
> cause integration friction across modules.

---

## 1. Mission

**Fodex** = **F**ield **O**perations & **D**istribution **EX**change

A modular monolith for **field sales and distribution management**:
distributors, dealers, subdealers, allocation, finance, CRM. On-prem
Kubernetes deployment, 3–4 distributors, tens of thousands of dealers,
~100–200 concurrent users.

**Repository:** https://github.com/lvin88/Fodex

---

## 2. Tech Stack (Authoritative)

| Concern | Choice | Notes |
|---|---|---|
| Runtime | .NET 10 | SDK 10.0.203, pinned via `global.json` |
| Language | C# 13 | file-scoped namespaces required |
| CQRS / Mediator | **Mediator 3.0.2** (martinothamar) | NOT MediatR (commercial as of v14) |
| Validation | FluentValidation 12.1.1 | Apache 2.0, free |
| ORM (writes) | EF Core 10 | per-module DbContext, schema-per-module |
| Reads | Dapper | for performance-critical queries |
| Database | PostgreSQL | UUID primary keys (UUIDv7) |
| Cache | Redis | per-module logical separation |
| Message broker | RabbitMQ + MassTransit | for integration events |
| Auth | ASP.NET Core Identity (password hasher only) + custom JWT | NOT Duende, NOT OpenIddict |
| API Gateway | YARP | reverse-proxy + auth aggregation |
| Tests | xUnit + FluentAssertions 6.12.x | NOT 8+ — commercial under Xceed |
| Architecture tests | NetArchTest.Rules 1.3.2 | enforces structural invariants |
| CI/CD | GitHub + Azure DevOps | source on GitHub, pipelines TBD |

**Pinned versions live in `Directory.Packages.props` (Central Package Management is enabled).**

---

## 3. Architectural Decisions (Non-Negotiable)

These five decisions shape every module. Never override them without
discussing with the human first.

### 3.1 Strongly-Typed IDs with UUIDv7

Every aggregate identifier is a `readonly record struct` wrapping
`Guid.CreateVersion7()`. UUIDv7 embeds a millisecond timestamp in its
leading bits, producing identifiers that are both globally unique and
monotonically ordered — critical for B-tree index health in PostgreSQL.

```csharp
// In Identity.Domain:
public readonly record struct UserId(Guid Value) : IStronglyTypedId
{
    public static UserId New() => new(StronglyTypedIdHelpers.NewSequentialId());
    public override string ToString() => Value.ToString();
}
```

- Marker interface: `IStronglyTypedId` (Guid-backed) or `IStronglyTypedId<T>` (generic).
- Generation helper: `Fodex.Common.Domain.Primitives.StronglyTypedIdHelpers.NewSequentialId()`.
- **Never** use `Guid.NewGuid()` in domain code — always `Guid.CreateVersion7()` or the helper.
- **Never** expose primitive `Guid` on aggregate boundaries. Always typed.

### 3.2 Result Pattern — No Exceptions for Business Outcomes

Business failures (validation errors, not-found, conflict) are returned
as `Result` / `Result<T>`. **Exceptions are reserved for genuinely
exceptional conditions** (database unreachable, programmer bugs,
constraint violations from a faulty caller).

```csharp
public static Result<User> Register(string emailInput, string fullName)
{
    var emailResult = Email.Create(emailInput);
    if (emailResult.IsFailure)
        return Result.Failure<User>(emailResult.Error);

    var user = new User(UserId.New(), emailResult.Value, fullName);
    user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id, emailResult.Value));
    return user;  // implicit conversion to Result<User>
}
```

- `Error` is a non-sealed `record` (so `ValidationError` and other
  specialized errors can extend it).
- `Error` carries `Code`, `Description`, `ErrorType` (enum: Failure,
  NotFound, Validation, Conflict, Unauthorized, Forbidden).
- Error codes follow `Module.ErrorName` convention: `User.NotFound`,
  `Order.InsufficientStock`.
- `Result<T>` provides `implicit operator Result<T>(T value)` — return
  `value` directly, no need to wrap with `Result.Success`.

### 3.3 Pure Domain Layer

`Fodex.Common.Domain` and every `Fodex.Modules.{Name}.Domain` have
**zero external dependencies**. No Mediator, no EF Core, no
FluentValidation, no Microsoft.AspNetCore, no logging frameworks.

This is enforced by `Fodex.ArchitectureTests`. Adding a forbidden
dependency to Domain will fail the build.

### 3.4 Adapter Pattern for Domain Events

Domain events are pure data. They do **not** implement Mediator's
`INotification`. An adapter wraps them at the application/infrastructure
boundary:

```
IDomainEvent (Common.Domain, pure)
    ↓ wrapped by
IDomainEventNotification<T> (Common.Application, marker)
    ↓ implemented by
DomainEventNotification<T> (Common.Infrastructure, concrete)
    ↓ dispatched via
Mediator INotificationHandler (in module application layers)
```

The `DomainEventNotification.Create(IDomainEvent)` static helper uses
reflection to construct the correctly-typed wrapper at runtime
(EF Core SaveChanges interceptor does this for every raised event).

### 3.5 Value Objects via Smart Constructors

Value objects are `sealed record` with **private constructors** and
static `Create` factories returning `Result<TValueObject>`. This makes
it impossible for an instance to exist in an invalid state.

```csharp
public sealed record Email : IValueObject
{
    public string Value { get; }
    private Email(string value) { Value = value; }

    public static Result<Email> Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result.Failure<Email>(EmailErrors.Empty);
        if (!IsValidFormat(input))
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        return new Email(input.Trim().ToLowerInvariant());
    }
}
```

- No `ValueObject` base class — `record` provides structural equality.
- Marker interface: `IValueObject`.

---

## 4. Mediator 3.0 — Critical Operational Notes

Mediator 3.0 (martinothamar) has **breaking changes** from MediatR and
from Mediator 2.x. These caused real pain during setup; do not relearn
them.

### 4.1 SourceGenerator Placement

- **`Mediator.SourceGenerator`** belongs **only in the host project**
  (`src/Host/Fodex.Api/`). It scans the compilation, finds handlers, and
  generates dispatch code. If it runs in a library project, it raises
  `MSG0005` for every `INotification`/`IRequest` whose handler lives
  in a different assembly.
- **`Mediator.Abstractions`** belongs in every project that defines
  message types or handlers (Common.Application, every module's
  Application/Infrastructure).

### 4.2 IPipelineBehavior Signature

Mediator 3.0 changed the signature:

```csharp
// CORRECT (Mediator 3.x):
public sealed class MyBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage   // ← required constraint
{
    public ValueTask<TResponse> Handle(
        TRequest message,
        MessageHandlerDelegate<TRequest, TResponse> next,  // ← NEW delegate type
        CancellationToken cancellationToken) { ... }
}

// WRONG (Mediator 2.x — does not compile):
RequestHandlerDelegate<TResponse> next   // gone
```

### 4.3 ICommand Name Clash

Mediator 3.0 ships its own `ICommand` interface in the `Mediator`
namespace. It clashes with our
`Fodex.Common.Application.Abstractions.Messaging.ICommand`. In any file
that uses **both** Mediator's pipeline types and our marker, add an
alias at the top:

```csharp
using Mediator;
using ICommand = Fodex.Common.Application.Abstractions.Messaging.ICommand;
```

Files that only reference our `ICommand<T>` (not the non-generic) do
not need the alias.

---

## 5. Naming Conventions (Enforced)

| Type | Suffix | Example |
|---|---|---|
| Command | `Command` | `RegisterUserCommand` |
| Query | `Query` | `GetUserByIdQuery` |
| Command/Query handler | `Handler` | `RegisterUserCommandHandler` |
| Validator | `Validator` | `RegisterUserCommandValidator` |
| Domain event | `DomainEvent` | `UserRegisteredDomainEvent` |
| Integration event | `IntegrationEvent` | `UserRegisteredIntegrationEvent` |
| Strongly-typed ID | `Id` | `UserId`, `OrderId` |
| Repository interface | `Repository` | `IUserRepository` |
| Errors static class | `Errors` | `UserErrors`, `EmailErrors` |

Architecture tests verify the suffixes for the most important kinds.

---

## 6. Module Structure (Per Bounded Context)

Each business module has **five projects**:

```
src/Modules/{Name}/
├── Fodex.Modules.{Name}.Domain/
│   ├── {Aggregate}/
│   │   ├── {Aggregate}.cs
│   │   ├── {Aggregate}Id.cs
│   │   ├── {ValueObject}.cs
│   │   ├── {Aggregate}Errors.cs
│   │   ├── {Event}DomainEvent.cs
│   │   └── I{Aggregate}Repository.cs
│   └── (more aggregates as the module grows)
│
├── Fodex.Modules.{Name}.Application/
│   ├── {Aggregate}/
│   │   ├── {UseCase}/
│   │   │   ├── {UseCase}Command.cs        (or Query)
│   │   │   ├── {UseCase}CommandValidator.cs
│   │   │   └── {UseCase}CommandHandler.cs
│   │   └── (more use cases)
│   └── Abstractions/        ← module-specific cross-cutting
│
├── Fodex.Modules.{Name}.Infrastructure/
│   ├── Persistence/
│   │   ├── {Name}DbContext.cs
│   │   ├── Configurations/{Aggregate}Configuration.cs
│   │   └── Repositories/{Aggregate}Repository.cs
│   ├── (other infra concerns: external services, etc.)
│   └── DependencyInjection.cs    ← Add{Name}Module() extension
│
├── Fodex.Modules.{Name}.Presentation/
│   └── Endpoints/
│       └── {Aggregate}/{UseCase}Endpoint.cs
│
└── Fodex.Modules.{Name}.IntegrationEvents/
    └── {Event}IntegrationEvent.cs    ← primitive types ONLY
```

**Per-module schema in PostgreSQL:** Identity → `identity` schema,
Sales → `sales` schema, etc. EF Core configurations set schema explicitly.

---

## 7. Cross-Module Communication

Modules **must not reference each other's Domain, Application, or
Infrastructure projects**. The only allowed dependency is:

```
Module A → Module B's IntegrationEvents project (read-only contract)
```

Even then, A consumes B's events through the message broker
(RabbitMQ/MassTransit), not by direct invocation. This guarantees that
extracting a module into a separate service later is a build-config
change, not a code rewrite.

**Integration event payloads are primitive types only:**

```csharp
// CORRECT
public sealed record UserRegisteredIntegrationEvent(
    Guid UserId,                // primitive — NOT UserId
    string Email,               // primitive — NOT Email value object
    string FullName,
    DateTime RegisteredAtUtc) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.CreateVersion7();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}
```

A consuming module cannot reference the publisher's `UserId` type — it
only knows `Guid`. This is enforced by an architecture test (planned).

---

## 8. Forbidden Patterns

These will be caught in code review or by architecture tests. Avoid
them in generated code:

- ❌ `throw new SomethingException(...)` for business failures —
  use `Result.Failure(...)` instead.
- ❌ Public constructors on aggregates or value objects — use static
  factories (`Register`, `Create`, etc.).
- ❌ `Guid.NewGuid()` for new entity IDs — use UUIDv7.
- ❌ `DateTime.UtcNow` directly in handlers/aggregates — inject `IClock`.
- ❌ Domain layer importing anything from Application or Infrastructure.
- ❌ Application importing Infrastructure.
- ❌ Integration events with strongly-typed IDs or value objects in
  their payload (consumers can't see those types).
- ❌ FluentAssertions 8.x or higher (commercial license).
- ❌ `Mediator.SourceGenerator` package reference in library projects.
- ❌ `sealed record Error(...)` — `Error` must remain inheritable.
- ❌ `IDomainEvent : INotification` — the whole point of the adapter
  pattern is that Domain stays free of Mediator.
- ❌ Repository methods returning `IQueryable<T>` — that leaks ORM
  semantics. Repositories return aggregates or `Maybe`/`Result`.
- ❌ `_ = await ...` without a meaningful reason — usually indicates
  swallowed errors.

---

## 9. Code Style

- File-scoped namespaces (`namespace X;`), not block-scoped.
- Explicit accessibility modifiers everywhere (including interface
  members) — enforced by `IDE0040`.
- `_camelCase` for private fields.
- `Async` suffix on all async methods returning `Task` / `ValueTask`.
- XML doc comments on public types in Common projects (modules can be
  more relaxed for internal types).
- `<inheritdoc />` on interface implementations to avoid doc duplication.
- Sealed by default — every class that doesn't need inheritance is `sealed`.

---

## 10. Working with This Codebase

When asked to add a new feature or module:

1. **Read the existing code first.** The patterns are consistent;
   match them. Look at `Fodex.Common.Application/Abstractions/Messaging`
   for the CQRS shape, `Fodex.Common.Domain/Primitives` for entities.
2. **Check `Directory.Packages.props`** before adding a NuGet
   reference. Use existing pinned versions.
3. **Run `dotnet build` and `dotnet test`** after changes. Architecture
   tests run automatically and catch most structural mistakes.
4. **Commit messages** follow Conventional Commits:
   `feat(identity): add user registration use case`,
   `fix(common): correct ValidationError null check`.
5. **Don't introduce new architectural patterns** without confirming
   with the human first. Stability beats novelty.

---

*Last updated: when Common.Domain, Common.Application, Common.Infrastructure,
and ArchitectureTests projects were green. First module (Identity) is next.*