# Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN mkdir -p /certificates && chmod 700 /certificates
RUN mkdir -p /keys && chmod 777 /keys
USER $APP_UID
WORKDIR /app
EXPOSE 443

# Build Stage: restore and build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["src/IdentityService/IdentityService.csproj", "src/IdentityService/"]
RUN dotnet restore "src/IdentityService/IdentityService.csproj"
COPY . .
WORKDIR "/src/src/IdentityService"
RUN dotnet build "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage: package the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: create the production image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY certificates/aspnetapp.pfx /certificates/aspnetapp.pfx

# Listen on HTTPS only
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "IdentityService.dll"]
