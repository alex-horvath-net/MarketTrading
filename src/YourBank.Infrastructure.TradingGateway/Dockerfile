# --------------------------------------------------
# Stage 1: Base Runtime Image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=1001 # Set a default UID if not provided
# Correct user creation for Debian-based images (default for dotnet images)
RUN useradd -u ${APP_UID} -m yourBankDockerUser
USER yourBankDockerUser
WORKDIR /app
EXPOSE 80

# --------------------------------------------------
# Stage 2: Restore Dependencies
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the .csproj file to leverage caching
COPY ["src/YourBank.Infrastructure.TradingGateway/YourBank.Infrastructure.TradingGateway.csproj", "YourBank.Infrastructure.TradingGateway/"]
RUN dotnet restore "./YourBank.Infrastructure.TradingGateway/YourBank.Infrastructure.TradingGateway.csproj"

# --------------------------------------------------
# Stage 3: Build the TradingGateway Project
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy restored files from restore stage
COPY --from=restore /src/YourBank.Infrastructure.TradingGateway/ /src/YourBank.Infrastructure.TradingGateway/
# Copy the rest of the source code
COPY . .
WORKDIR "/src/YourBank.Infrastructure.TradingGateway"
# Build the project and output the build artifacts
RUN dotnet build "./YourBank.Infrastructure.TradingGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------------------------------
# Stage 4: Publish the TradingGateway Application
# --------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/YourBank.Infrastructure.TradingGateway"
RUN dotnet publish "./YourBank.Infrastructure.TradingGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --------------------------------------------------
# Stage 5: Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.TradingGateway.dll"]
