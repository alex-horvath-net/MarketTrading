# Stage 0: Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443

# Stage 1: Restore and Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TradingPortal.BlazorWebApp/TradingPortal.BlazorWebApp.csproj", "src/TradingPortal.BlazorWebApp/"]
RUN dotnet restore "./src/TradingPortal.BlazorWebApp/TradingPortal.BlazorWebApp.csproj"
COPY . .
WORKDIR "/src//src/TradingPortal.BlazorWebApp"
RUN dotnet build "./TradingPortal.BlazorWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TradingPortal.BlazorWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingPortal.BlazorWebApp.dll"]

# Use HTTPS in Docker
ENV ASPNETCORE_URLS="https://+:443"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Alex!1978 