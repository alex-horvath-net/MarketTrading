# Stage 1: Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443

# Stage 2: Build & Restore
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/TradingPortal.Blazor/TradingPortal.Blazor.csproj", "TradingPortal.Blazor/"]
RUN dotnet restore "TradingPortal.Blazor/TradingPortal.Blazor.csproj"
COPY . .
WORKDIR "/src/TradingPortal.Blazor"
RUN dotnet publish "TradingPortal.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime Image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Use HTTPS in Docker
ENV ASPNETCORE_URLS="https://+:443"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=""

ENTRYPOINT ["dotnet", "TradingPortal.Blazor.dll"]
