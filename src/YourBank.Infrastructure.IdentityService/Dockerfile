#running the app from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base # Create base image from the official ASP.NET Core runtime image (9.0).
USER $APP_UID # Set the user to the one created in docker compose file
WORKDIR /app # set the working directory to app inside the container
EXPOSE 80


# This stage is used to build the service project
# Stage 2: Build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build # Create build image from the official ASP.NET Core SDK image (9.0)
ARG BUILD_CONFIGURATION=Release # Set the build configuration to Release by default
WORKDIR /src # set the working directory src inside the container
# Copy the phisical file (relative fro .sln) to the container\src/YourBank.Infrastructure.IdentityService
COPY ["src/YourBank.Infrastructure.IdentityService/YourBank.Infrastructure.IdentityService.csproj", "YourBank.Infrastructure.IdentityService/"] 
RUN dotnet restore "./YourBank.Infrastructure.IdentityService/YourBank.Infrastructure.IdentityService.csproj" # Restore the service project
COPY . . # Copy the rest of the files to the working directory
WORKDIR "/src/YourBank.Infrastructure.IdentityService" # Change the working directory to the service project
RUN dotnet build "./YourBank.Infrastructure.IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build # Build the service project

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YourBank.Infrastructure.IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final # Use the base image as the final image
WORKDIR /app # Set the working directory inside the container
COPY --from=publish /app/publish . # Copy the published files to the working directory
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.IdentityService.dll"] # Set the entry point for the container