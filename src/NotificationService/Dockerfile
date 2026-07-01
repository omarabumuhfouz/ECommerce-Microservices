# Step 1: Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Step 2: Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["NotificationService.csproj", "./"]
RUN dotnet restore "./NotificationService.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/."

# Build the project
RUN dotnet build "NotificationService.csproj" -c Release -o /app/build

# Step 3: Publish the application
FROM build AS publish
RUN dotnet publish "NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Step 4: Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]