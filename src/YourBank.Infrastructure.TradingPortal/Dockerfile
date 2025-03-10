# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID # Set the APP_UID argument to the value passed in the docker-compose file
USER $APP_UID # Set the user to the one created in docker compose file
RUN useradd -u ${APP_UID} -m myappuser # Create a non-root user with the specified UID and a home directory.
WORKDIR /app
EXPOSE 80

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["YourBank.Infrastructure.TradingPortal/YourBank.Infrastructure.TradingPortal.csproj", "YourBank.Infrastructure.TradingPortal/"]
RUN dotnet restore "./YourBank.Infrastructure.TradingPortal/YourBank.Infrastructure.TradingPortal.csproj"
COPY . .
WORKDIR "/src/YourBank.Infrastructure.TradingPortal"
RUN dotnet build "./YourBank.Infrastructure.TradingPortal.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YourBank.Infrastructure.TradingPortal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBank.Infrastructure.TradingPortal.dll"]
