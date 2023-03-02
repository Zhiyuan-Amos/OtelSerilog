# OpenTelemetry & Serilog

## Getting Started

### Initializing Services

1. Navigate to `apm-server` and run `docker compose up`. This initializes Elastic Stack which consists of Observability Frontend & Backend.
2. Run `dotnet run --project Caller/Caller.csproj`.
3. Run `dotnet run --project Callee/Callee.csproj`.

### Emitting Logs & Traces

1. Run `curl http://localhost:5144`. `Caller` calls `Callee` and the operation is successful.
2. Run `curl http://localhost:5144/error`. `Caller` calls `Callee` and `Callee` returns an error.
3. Run `curl http://localhost:5144/post`. `Caller` calls `Callee` with a valid object in the Request Body and the operation is successful.
4. Run `curl http://localhost:5144/post/error`. `Caller` calls `Callee` with an invalid object in the Request Body and `Callee` returns an error.

### Viewing Logs & Traces

1. Navigate to `http://localhost:5601` and login with the user `admin` and password `changeme`.

## Concerns

Some NuGET packages do not have a stable release yet.

## FAQ

1. Question: What are the benefits of Structured Logging?

    Answer: See [StackOverflow answer](https://softwareengineering.stackexchange.com/a/312586).

2. Question: What are the benefits of Serilog over Microsoft's default logger?

    Answer: Filtering can be done more easily using `ExpressionTemplate` compared to Microsoft's implementation which is more [verbose](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0#filter-function).

