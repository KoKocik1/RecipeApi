# Use the .NET Core SDK 8.0 image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the working directory
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image (this part depends on your specific needs)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published application to the container
COPY --from=build /app/out .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "RecipeApi.dll"]
