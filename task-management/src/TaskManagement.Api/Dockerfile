# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file from parent directory
COPY ["../TaskManagement.sln", "./"]
COPY ["../TaskManagement.Application/TaskManagement.Application.csproj", "TaskManagement.Application/"]
COPY ["../TaskManagement.Domain/TaskManagement.Domain.csproj", "TaskManagement.Domain/"]
COPY ["../TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj", "TaskManagement.Infrastructure/"]

# Copy current project file
COPY ["TaskManagement.Api.csproj", "./"]

# Restore dependencies
RUN dotnet restore "TaskManagement.Api.csproj"

# Copy all source code
COPY . ./TaskManagement.Api/
COPY ../TaskManagement.Application/. ./TaskManagement.Application/
COPY ../TaskManagement.Domain/. ./TaskManagement.Domain/
COPY ../TaskManagement.Infrastructure/. ./TaskManagement.Infrastructure/

# Build the project
WORKDIR "/src/TaskManagement.Api"
RUN dotnet build "TaskManagement.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TaskManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Copy published files
COPY --from=publish /app/publish .

# Create database directory
RUN mkdir -p /app/data

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TaskManagement.Api.dll"]
