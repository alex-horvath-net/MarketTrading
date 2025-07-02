# MarketTrading is the base host folder

# Base Stage: base image is a linux server with .net sdk 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Create and secure the certificates directory (root)
RUN mkdir -p /certificates && chmod 700 /certificates

# Create and secure the data-protection keys directory (root)
RUN mkdir -p /keys && chmod 777 /keys

# Switch to the unprivileged ASP.NET user
USER $APP_UID

# base/app is the container base folder
WORKDIR /app
EXPOSE 443

# Build Stage: restore and build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# build/src is the container base folder 
WORKDIR /src

# from MarketTrading/NuGet.Config 
# to   build/src/NuGet.Config
COPY ["NuGet.Config", "./"]

# from MarketTrading/src/TradingService/TradingService.csproj
# to   build/src/src/TradingService/TradingService.csproj
COPY ["src/TradingService/TradingService.csproj", "src/TradingService/"]

RUN dotnet restore "src/TradingService/TradingService.csproj"

# from MarketTrading/*
# to   build/src/*
COPY . .

# build/src/src/TradingService is the container base folder 
WORKDIR "/src/src/TradingService"

RUN dotnet build "TradingService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage: package the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TradingService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: create the production image
FROM base AS final
WORKDIR /app

# Copy the published app
COPY --from=publish /app/publish .

# Copy the HTTPS development certificate into the container
COPY certificates/aspnetapp.pfx /certificates/aspnetapp.pfx

# Listen on HTTPS only
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "TradingService.dll"]
