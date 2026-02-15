# 1. Base Image (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7000

# 2. Build Image (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# --- COPY CSPROJ FILES (For Layer Caching) ---
# We assume the build context is the Repo Root (..) so we can reach SharedKernel

# Main Service Projects
COPY ["src/AuthService/AuthService.Api/AuthService.Api.csproj", "src/AuthService/AuthService.Api/"]
COPY ["src/AuthService/AuthService.Application/AuthService.Application.csproj", "src/AuthService/AuthService.Application/"]
COPY ["src/AuthService/AuthService.Domain/AuthService.Domain.csproj", "src/AuthService/AuthService.Domain/"]
COPY ["src/AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "src/AuthService/AuthService.Infrastructure/"]

# Shared Projects (Based on your architecture)
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/Contracts/Contracts.csproj", "src/Contracts/"]

# --- RESTORE ---
RUN dotnet restore "src/AuthService/AuthService.Api/AuthService.Api.csproj"

# --- COPY EVERYTHING ELSE ---
COPY . .

# --- BUILD & PUBLISH ---
WORKDIR "/src/src/AuthService/AuthService.Api"
RUN dotnet build "AuthService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]