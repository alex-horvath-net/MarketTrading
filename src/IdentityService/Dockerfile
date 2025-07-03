# Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN mkdir -p /certificates && chmod 700 /certificates
USER $APP_UID
WORKDIR /app
EXPOSE 443

# Build Stage: restore and build application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["src/IdentityService/IdentityService.csproj", "src/IdentityService/"]
RUN dotnet restore "src/IdentityService/IdentityService.csproj"

# copy everything, including your certificates folder
COPY . .


# build the project
WORKDIR "/src/src/IdentityService"
RUN dotnet build "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage: package the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage: create the production image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy the PFX into the container
COPY certificates/aspnetapp.pfx /certificates/aspnetapp.pfx

# Listen on HTTPS only
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "IdentityService.dll"]
