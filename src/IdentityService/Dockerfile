FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY NuGet.Config ./
COPY src/IdentityService/IdentityService.csproj ./
RUN dotnet restore IdentityService.csproj
COPY src/IdentityService/ ./
RUN dotnet build IdentityService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish IdentityService.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app/publish .  
COPY certificates/aspnetapp.pfx ./certificates/aspnetapp.pfx

ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "IdentityService.dll"]
