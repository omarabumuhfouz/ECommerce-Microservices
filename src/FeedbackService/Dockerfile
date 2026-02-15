FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. OPTIMIZATION: Copy ONLY the .csproj files first!
# You need to copy SharedKernel too since FeedbackService depends on it
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
COPY ["src/FeedbackService/FeedbackService.Domain/FeedbackService.Domain.csproj", "src/FeedbackService/FeedbackService.Domain/"]
COPY ["src/FeedbackService/FeedbackService.Application/FeedbackService.Application.csproj", "src/FeedbackService/FeedbackService.Application/"]
COPY ["src/FeedbackService/FeedbackService.Infrastructure/FeedbackService.Infrastructure.csproj", "src/FeedbackService/FeedbackService.Infrastructure/"]
COPY ["src/FeedbackService/FeedbackService.Api/FeedbackService.Api.csproj", "src/FeedbackService/FeedbackService.Api/"]

# 2. OPTIMIZATION: Run Restore NOW (This layer will be CACHED)
RUN dotnet restore "src/FeedbackService/FeedbackService.Api/FeedbackService.Api.csproj"

# 3. NOW copy the rest of the source code
COPY . .

# 4. Build and Publish
WORKDIR "/app/src/FeedbackService/FeedbackService.Api"
RUN dotnet publish "FeedbackService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FeedbackService.Api.dll"]