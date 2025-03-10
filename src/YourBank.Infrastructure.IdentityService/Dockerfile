# Stage1:running the app from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base # Create base image from the official ASP.NET Core runtime image (9.0).
ARG APP_UID # Set the APP_UID argument to the value passed in the docker-compose file
USER $APP_UID # Set the user to the one created in docker compose file
RUN useradd -u ${APP_UID} -m myappuser # Create a non-root user with the specified UID and a home directory.
USER myappuser # Switch to the newly created user.
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

# Stage 2: Restore Dependencies
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore # Use the .NET SDK image for restoring packages
ARG BUILD_CONFIGURATION=Release # Set the build configuration to Release by default
WORKDIR /src # Set the working directory to /src inside the container
# copy the .csproj (relative path from .sln) to workdir + p2  
COPY ["src/YourBank.Infrastructure.IdentityService/YourBank.Infrastructure.IdentityService.csproj", "YourBank.Infrastructure.IdentityService/"]
# Run dotnet restore to cache NuGet dependencies; if the .csproj hasn't changed, this layer is cached.
RUN dotnet restore "./YourBank.Infrastructure.IdentityService/YourBank.Infrastructure.IdentityService.csproj"

# Stage 3: Build the Service Project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy the project file and restored dependencies 
# from restoreStage/src/YourBank.Infrastructure.IdentityService/ to buildStage/src/YourBank.Infrastructure.IdentityService/
COPY --from=restore /src/YourBank.Infrastructure.IdentityService/ "YourBank.Infrastructure.IdentityService/"
COPY . . # Copy the rest of the source code into the container.
WORKDIR "/src/YourBank.Infrastructure.IdentityService" # Change to the project directory to the folder of .csproj.
# Build the project uand save tehe result to /app/build.
RUN dotnet build "./YourBank.Infrastructure.IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Stage3: This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YourBank.Infrastructure.IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage4: This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final # Use the base image as the final image
WORKDIR /app # Set the working directory inside the container
COPY --from=publish /app/publish . # Copy the published files to the working directory
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.IdentityService.dll"] # Set the entry point for the container