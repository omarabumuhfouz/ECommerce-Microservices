FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. OPTIMIZATION: Copy ONLY the .csproj files first!
# You need to copy SharedKernel too since PaymentService depends on it
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/PaymentService/PaymentService.Domain/PaymentService.Domain.csproj", "src/PaymentService/PaymentService.Domain/"]
COPY ["src/PaymentService/PaymentService.Application/PaymentService.Application.csproj", "src/PaymentService/PaymentService.Application/"]
COPY ["src/PaymentService/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj", "src/PaymentService/PaymentService.Infrastructure/"]
COPY ["src/PaymentService/PaymentService.Api/PaymentService.Api.csproj", "src/PaymentService/PaymentService.Api/"]

# 2. OPTIMIZATION: Run Restore NOW (This layer will be CACHED)
RUN dotnet restore "src/PaymentService/PaymentService.Api/PaymentService.Api.csproj"

# 3. NOW copy the rest of the source code
COPY . .

# 4. Build and Publish
WORKDIR "/app/src/PaymentService/PaymentService.Api"
RUN dotnet publish "PaymentService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.Api.dll"]