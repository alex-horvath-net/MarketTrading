# MarketTrading is the base host folder

# Base Stage: base image is a linux server with .net sdk 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
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

# from MarketTrading/src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj
# to   build/src/src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj
COPY ["src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj", "src/TradingPortal.BlazorServer/"]

RUN dotnet restore "src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj"

# from MarketTrading/*
# to   build/src/*
COPY . .

# build/src/src/TradingPortal.BlazorServer is the container base folder 
WORKDIR "/src/src/TradingPortal.BlazorServer"

RUN dotnet build "TradingPortal.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TradingPortal.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "TradingPortal.BlazorServer.dll"]