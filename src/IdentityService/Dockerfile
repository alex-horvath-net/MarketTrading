# Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
RUN mkdir -p /certificates && chmod 755 /certificates
WORKDIR /app
EXPOSE 443

# Build Stage: restore and build application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["src/IdentityService/IdentityService.csproj", "src/IdentityService/"]
RUN dotnet restore "src/IdentityService/IdentityService.csproj"
COPY . .
WORKDIR "/src/src/IdentityService"
RUN dotnet build "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage: package the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage (embed PFX into image)
FROM base AS final
USER root
WORKDIR /app

# 1) Copy published output
COPY --from=publish /app/publish .

# 2) Embed the PFX by climbing out to the repo-level Certificates folder
#    Relative from src/IdentityService: two levels up to MarketTrading, then into Certificates
COPY ../../Certificates/aspnetapp.pfx /certificates/aspnetapp.pfx

# 3) Set permissions so the non-root app user can read the PFX
RUN chmod 644 /certificates/aspnetapp.pfx && chown $APP_UID:$APP_GID /certificates/aspnetapp.pfx

# Switch back to app user
USER $APP_UID

# Only HTTPS on port 443
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "IdentityService.dll"]
