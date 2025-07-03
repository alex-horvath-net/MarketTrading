# Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
RUN mkdir -p /certificates && chmod 755 /certificates
WORKDIR /app
EXPOSE 443

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["src/IdentityService/IdentityService.csproj", "src/IdentityService/"]
RUN dotnet restore "src/IdentityService/IdentityService.csproj"
COPY . .
WORKDIR "/src/src/IdentityService"
RUN dotnet build "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage (embed PFX into image)
FROM base AS final
USER root
WORKDIR /app

# 1) Copy published output
COPY --from=publish /app/publish .

# 2) Embed the PFX from the lowercase 'certificates' folder in your repo:
#    From context (src/IdentityService) go up two levels into the repo root.
COPY ../../certificates/aspnetapp.pfx /certificates/aspnetapp.pfx

# 3) Fix permissions so the non-root app user can read the PFX
RUN chmod 644 /certificates/aspnetapp.pfx \
 && chown $APP_UID:$APP_GID /certificates/aspnetapp.pfx

# Switch back to non-root app user
USER $APP_UID

# Only HTTPS on port 443
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "IdentityService.dll"]
