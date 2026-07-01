# Step 1: Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Step 2: Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["EmailService.Api.csproj", "./"]
RUN dotnet restore "./EmailService.Api.csproj"

# Copy the remaining source code
COPY . .
WORKDIR "/src/."

# Build the project
RUN dotnet build "EmailService.Api.csproj" -c Release -o /app/build

# Step 3: Publish
FROM build AS publish
RUN dotnet publish "EmailService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Step 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailService.Api.dll"]