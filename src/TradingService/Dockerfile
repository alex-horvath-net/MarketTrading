FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY NuGet.Config ./
COPY src/TradingService/TradingService.csproj ./
RUN dotnet restore TradingService.csproj
COPY src/TradingService/ ./
RUN dotnet build TradingService.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish TradingService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .  

ENV ASPNETCORE_URLS="http://+:80"
ENTRYPOINT ["dotnet", "TradingService.dll"]
