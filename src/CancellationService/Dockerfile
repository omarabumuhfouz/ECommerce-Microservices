FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. OPTIMIZATION: Copy ONLY the .csproj files first!
# You need to copy SharedKernel too since CancellationService depends on it
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/CancellationService/CancellationService.Domain/CancellationService.Domain.csproj", "src/CancellationService/CancellationService.Domain/"]
COPY ["src/CancellationService/CancellationService.Application/CancellationService.Application.csproj", "src/CancellationService/CancellationService.Application/"]
COPY ["src/CancellationService/CancellationService.Infrastructure/CancellationService.Infrastructure.csproj", "src/CancellationService/CancellationService.Infrastructure/"]
COPY ["src/CancellationService/CancellationService.Api/CancellationService.Api.csproj", "src/CancellationService/CancellationService.Api/"]

# 2. OPTIMIZATION: Run Restore NOW (This layer will be CACHED)
RUN dotnet restore "src/CancellationService/CancellationService.Api/CancellationService.Api.csproj"

# 3. NOW copy the rest of the source code
COPY . .

# 4. Build and Publish
WORKDIR "/app/src/CancellationService/CancellationService.Api"
RUN dotnet publish "CancellationService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CancellationService.Api.dll"]