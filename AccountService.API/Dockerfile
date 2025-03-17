# Stage 1: Build application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY ../AccountService.sln ./
COPY ../AccountService.API/*.csproj ./AccountService.API/
COPY ../AccountService.Application/*.csproj ./AccountService.Application/
COPY ../AccountService.Core/*.csproj ./AccountService.Core/
COPY ../AccountService.Persistence/*.csproj ./AccountService.Persistence/
COPY ../AccountService.Infrastructure/*.csproj ./AccountService.Infrastructure/
RUN dotnet restore

# Copy everything and build the project
COPY ../ ./
RUN dotnet publish AccountService.API/AccountService.API.csproj -c Debug -o /app/publish

# Stage 2: Run application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy built application from the build stage
COPY --from=build /app/publish .

# Expose API port
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "AccountService.API.dll"]
