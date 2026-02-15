FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. Copy .csproj files for all dependencies
# This allows Docker to cache the 'dotnet restore' layer
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/Contracts/Contracts.csproj", "src/Contracts/"]
COPY ["src/ShoppingCartService/ShoppingCartService.Domain/ShoppingCartService.Domain.csproj", "src/ShoppingCartService/ShoppingCartService.Domain/"]
COPY ["src/ShoppingCartService/ShoppingCartService.Application/ShoppingCartService.Application.csproj", "src/ShoppingCartService/ShoppingCartService.Application/"]
COPY ["src/ShoppingCartService/ShoppingCartService.Infrastructure/ShoppingCartService.Infrastructure.csproj", "src/ShoppingCartService/ShoppingCartService.Infrastructure/"]
COPY ["src/ShoppingCartService/ShoppingCartService.Api/ShoppingCartService.Api.csproj", "src/ShoppingCartService/ShoppingCartService.Api/"]

# 2. Restore dependencies
RUN dotnet restore "src/ShoppingCartService/ShoppingCartService.Api/ShoppingCartService.Api.csproj"

# 3. Copy the rest of the source code
COPY . .

# 4. Build and Publish
WORKDIR "/app/src/ShoppingCartService/ShoppingCartService.Api"
RUN dotnet publish "ShoppingCartService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Standardize ports for Microservices
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "ShoppingCartService.Api.dll"]