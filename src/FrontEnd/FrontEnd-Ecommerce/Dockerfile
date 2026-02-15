# Use the official .NET 9 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY FrontEnd-Ecommerce.csproj ./
RUN dotnet restore FrontEnd-Ecommerce.csproj

# Copy the rest of the project
COPY . .

# Publish the application
RUN dotnet publish FrontEnd-Ecommerce.csproj -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port your app runs on
EXPOSE 9090

# Set the entry point
ENTRYPOINT ["dotnet", "FrontEnd-Ecommerce.dll"]
