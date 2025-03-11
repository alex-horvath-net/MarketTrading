# --------------------------------------------------
# Stage 1: Base Runtime Image (For Execution)
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID  # Set the APP_UID argument to the value passed in the docker-compose file

# Correct user creation for Debian-based images (default for .NET official images)
RUN useradd -u ${APP_UID} -m yourBankDockerUser
# Switch to the non-root user for better security
USER yourBankDockerUser
WORKDIR /app
EXPOSE 80

# --------------------------------------------------
# Stage 2: Restore Dependencies
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the .csproj file to optimize layer caching
COPY ["src/YourBank.Infrastructure.RiskService/YourBank.Infrastructure.RiskService.csproj", "YourBank.Infrastructure.RiskService/"]
RUN dotnet restore "./YourBank.Infrastructure.RiskService/YourBank.Infrastructure.RiskService.csproj"

# --------------------------------------------------
# Stage 3: Build the Service Project
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy restored project dependencies from restore stage
COPY --from=restore /src/YourBank.Infrastructure.RiskService/ /src/YourBank.Infrastructure.RiskService/
# Copy the remaining source files into the container
COPY . .
WORKDIR "/src/YourBank.Infrastructure.RiskService"
# Build the project and store output in /app/build
RUN dotnet build "./YourBank.Infrastructure.RiskService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------------------------------
# Stage 4: Publish the Service Project
# --------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/YourBank.Infrastructure.RiskService"
RUN dotnet publish "./YourBank.Infrastructure.RiskService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --------------------------------------------------
# Stage 5: Final Runtime Image (For Execution)
# --------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.RiskService.dll"]
