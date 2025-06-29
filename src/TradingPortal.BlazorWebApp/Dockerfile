# --------------------------------------------------
# Stage 1: Base Runtime Image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443

# --------------------------------------------------
# Stage 2: Restore and Publish
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./TradingPortal.BlazorWebApp.csproj", "TradingPortal.BlazorWebApp/"]
RUN dotnet restore "TradingPortal.BlazorWebApp/TradingPortal.BlazorWebApp.csproj"
COPY . .
WORKDIR "/src/TradingPortal.BlazorWebApp"
RUN dotnet publish "TradingPortal.BlazorWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# --------------------------------------------------
# Stage 3: Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Use HTTPS in Docker
ENV ASPNETCORE_URLS="https://+:443"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Alex!1978

ENTRYPOINT ["dotnet", "TradingPortal.BlazorWebApp.dll"]
