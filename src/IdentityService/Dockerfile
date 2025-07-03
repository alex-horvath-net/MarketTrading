FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
RUN mkdir -p /certificates && chmod 755 /certificates
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["src/IdentityService/IdentityService.csproj", "src/IdentityService/"]
RUN dotnet restore "src/IdentityService/IdentityService.csproj"
COPY . .
WORKDIR "/src/src/IdentityService"
RUN dotnet build "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app/publish .
COPY ../../Certificates/aspnetapp.pfx /certificates/aspnetapp.pfx
RUN chmod 644 /certificates/aspnetapp.pfx && chown $APP_UID:$APP_GID /certificates/aspnetapp.pfx
USER $APP_UID
ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "IdentityService.dll"]
