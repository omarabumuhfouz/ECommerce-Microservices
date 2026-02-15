FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. OPTIMIZATION: Copy ONLY the .csproj files first!
# You need to copy SharedKernel too since CustomerService depends on it
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/CustomerService/CustomerService.Domain/CustomerService.Domain.csproj", "src/CustomerService/CustomerService.Domain/"]
COPY ["src/CustomerService/CustomerService.Application/CustomerService.Application.csproj", "src/CustomerService/CustomerService.Application/"]
COPY ["src/CustomerService/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "src/CustomerService/CustomerService.Infrastructure/"]
COPY ["src/CustomerService/CustomerService.Api/CustomerService.Api.csproj", "src/CustomerService/CustomerService.Api/"]

# 2. OPTIMIZATION: Run Restore NOW (This layer will be CACHED)
RUN dotnet restore "src/CustomerService/CustomerService.Api/CustomerService.Api.csproj"

# 3. NOW copy the rest of the source code
COPY . .

# 4. Build and Publish
WORKDIR "/app/src/CustomerService/CustomerService.Api"
RUN dotnet publish "CustomerService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.Api.dll"]