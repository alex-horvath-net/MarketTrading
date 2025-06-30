# See https://aka.ms/customizecontainer to learn how to customize your debug container

# Runtime base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Expose HTTPS port only
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Use solution-level NuGet.Config
COPY ["NuGet.Config", "."]

# Copy project file and restore
COPY ["src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj", "src/TradingPortal.BlazorServer/"]
RUN dotnet restore "./src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj"

# Copy all source and build
COPY . .
WORKDIR "/src/src/TradingPortal.BlazorServer"
RUN dotnet build "TradingPortal.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TradingPortal.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Listen on HTTPS only
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "TradingPortal.BlazorServer.dll"]
