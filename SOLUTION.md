# Solution Documentation

## Problems Identified (top 3) and Fixes
1. Tight coupling and mixed responsibilities
   - Problem: Controllers instantiate concrete services and `TodoService` mixes business logic with data access.
   - Fix: Introduce `ITodoService` and `ITodoRepository`; move DB access into `SqliteTodoRepository`; inject `ITodoService` into controllers via DI.

2. SQL injection and unsafe data access
   - Problem: Inline SQL with string interpolation risks SQL injection and is hard to maintain.
   - Fix: Use parameterized queries (async ADO.NET, Dapper or EF Core). Move connection string to `appsettings.json` and use `IConfiguration` to bind it.

3. Poor testability and brittle tests
   - Problem: Tests and controllers depend on concrete classes and the real DB.
   - Fix: Depend on abstractions (`ITodoService`/`ITodoRepository`) so tests can mock those interfaces. Use in-memory SQLite for integration tests.

## Architectural Decisions
- Layered architecture: API (controllers) → Service (business rules) → Repository (data access).
- Patterns used:
  - Repository pattern for DB encapsulation (`ITodoRepository` / `SqliteTodoRepository`).
  - DI (built-in ASP.NET Core) for `ITodoService` / `ITodoRepository` registrations in `Program.cs`.
  - DTOs + model validation for API surface separation.
- Why:
  - Improves testability, single responsibility, and makes swapping data stores trivial.
  - Enables safe parameterized queries and centralized configuration.

## How to Run
Prerequisites
- .NET 8 SDK
- Visual Studio 2026 or CLI (PowerShell preferred)

Build
- From repository root:
  - `dotnet build ./TodoApi`

Run
- CLI:
  - `dotnet run --project ./TodoApi`
- Visual Studio:
  - Use `__Debug > Start Debugging__` or `__Debug > Start Without Debugging__`

Notes
- Move DB connection string to `appsettings.json` before running; app creates `todos.db` on first run for SQLite.

Test
- From repository root:
  - `dotnet test`

## API Documentation (recommended RESTful routes)
Base path: `/api/todos`

1. Create TODO
   - Method: POST
   - URL: `/api/todos`
   - Request Body: `{"task":"Learn ASP.NET Core","dueDate":"2023-12-31","isCompleted":false}`
   - Response: `201 Created`

2. Get TODO(s)
   - Method: GET
   - URL: `/api/todos` or `/api/todos/{id}`
   - Request: N/A or URL parameter (e.g., `/api/todos/1`)
   - Response: `200 OK` with TODO item(s) in body

3. Update TODO
   - Method: PUT
   - URL: `/api/todos/{id}`
   - Request Body: `{"task":"Learn ASP.NET Core Updated","dueDate":"2023-12-31","isCompleted":true}`
   - Response: `204 No Content`

4. Delete TODO
   - Method: DELETE
   - URL: `/api/todos/{id}`
   - Request: URL parameter (e.g., `/api/todos/1`)
   - Response: `204 No Content`

## Future Improvements
- Enhance authentication and authorization
- Implement rate limiting and caching
- Improve API documentation and examples
- Add support for user-specific TODO lists
- Enable deployment to cloud platforms with CI/CD
