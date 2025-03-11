# --------------------------------------------------
# Stage 1: Base Runtime Image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=1001  # Set a default UID if not provided
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

# Copy only the .csproj file to leverage caching
COPY ["src/YourBank.Infrastructure.TradingPortal/YourBank.Infrastructure.TradingPortal.csproj", "YourBank.Infrastructure.TradingPortal/"]
RUN dotnet restore "./YourBank.Infrastructure.TradingPortal/YourBank.Infrastructure.TradingPortal.csproj"

# --------------------------------------------------
# Stage 3: Build the TradingPortal Project
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy restored files from restore stage
COPY --from=restore /src/YourBank.Infrastructure.TradingPortal/ /src/YourBank.Infrastructure.TradingPortal/
# Copy the rest of the source code
COPY . .
WORKDIR "/src/YourBank.Infrastructure.TradingPortal"
# Build the project and store output in /app/build
RUN dotnet build "./YourBank.Infrastructure.TradingPortal.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------------------------------
# Stage 4: Publish the TradingPortal Application
# --------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/YourBank.Infrastructure.TradingPortal"
RUN dotnet publish "./YourBank.Infrastructure.TradingPortal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --------------------------------------------------
# Stage 5: Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.TradingPortal.dll"]
