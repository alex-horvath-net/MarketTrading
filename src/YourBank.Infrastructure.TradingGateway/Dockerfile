# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID # Set the APP_UID argument to the value passed in the docker-compose file
USER $APP_UID # Set the user to the one created in docker compose file
RUN useradd -D -u ${APP_UID} -m yourBankDockerUser # Create a non-root user with the specified UID and a home directory.
USER yourBankDockerUser # Switch to the newly created user.
WORKDIR /app # set the working directory to app inside the container
EXPOSE 80

# Stage 2: Restore Dependencies
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore # Use the .NET SDK image for restoring packages
ARG BUILD_CONFIGURATION=Release # Set the build configuration to Release by default
WORKDIR /src # Set the working directory to /src inside the container
# copy the .csproj (relative path from .sln) to workdir + p2  
COPY ["src/YourBank.Infrastructure.TradingGateway/YourBank.Infrastructure.TradingGateway.csproj", "YourBank.Infrastructure.TradingGateway/"]
# Run dotnet restore to cache NuGet dependencies; if the .csproj hasn't changed, this layer is cached.
RUN dotnet restore "./YourBank.Infrastructure.TradingGateway/YourBank.Infrastructure.TradingGateway.csproj"

# Stage 3: Build the Service Project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy the project file and restored dependencies 
# from restoreStage/src/YourBank.Infrastructure.TradingGateway/ to buildStage/src/YourBank.Infrastructure.TradingGateway/
COPY --from=restore /src/YourBank.Infrastructure.TradingGateway/ "YourBank.Infrastructure.TradingGateway/"
COPY . . # Copy the rest of the source code into the container.
WORKDIR "/src/YourBank.Infrastructure.TradingGateway" # Change to the project directory to the folder of .csproj.
# Build the project uand save tehe result to /app/build.
RUN dotnet build "./YourBank.Infrastructure.TradingGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YourBank.Infrastructure.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.Gateway.dll"]
