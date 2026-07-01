FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. OPTIMIZATION: Copy ONLY the .csproj files first!
# SharedKernel is copied since InventoryService likely depends on it
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/InventoryService/InventoryService.Domain/InventoryService.Domain.csproj", "src/InventoryService/InventoryService.Domain/"]
COPY ["src/InventoryService/InventoryService.Application/InventoryService.Application.csproj", "src/InventoryService/InventoryService.Application/"]
COPY ["src/InventoryService/InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "src/InventoryService/InventoryService.Infrastructure/"]
COPY ["src/InventoryService/InventoryService.Api/InventoryService.Api.csproj", "src/InventoryService/InventoryService.Api/"]

# 2. OPTIMIZATION: Run Restore NOW (This layer will be CACHED)
RUN dotnet restore "src/InventoryService/InventoryService.Api/InventoryService.Api.csproj"

# 3. NOW copy the rest of the source code
COPY . .

# 4. Build and Publish
WORKDIR "/app/src/InventoryService/InventoryService.Api"
RUN dotnet publish "InventoryService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.Api.dll"]