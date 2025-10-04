# Rate Limiter Service

A distributed rate limiting service built with .NET Core and Redis using sliding window algorithm.

## Architecture

- **RateLimiter.API**: ASP.NET Core Web API that exposes HTTP endpoints
- **RateLimiter.Core**: Core business logic including rate limiting algorithms
- **Redis**: Distributed cache for storing request counts and timestamps

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Docker Desktop
- Redis (via Docker)

### Running Locally

1. Start Redis:
```bash
   docker-compose up -d redis
```
2. Run the API:
```
  bash   dotnet run --project src/RateLimiter.API
```
3. Run tests:
```
  bash   dotnet test
```

### Using Docker
Build and run everything with Docker Compose:
```
  bashdocker-compose up --build
```
