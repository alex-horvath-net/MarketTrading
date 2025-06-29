# Stage 0: Base Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443


# Stage 1: Restore & Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
# set container base folder to src
WORKDIR /src
# Copy only .csproj 
COPY TradingPortal.BlazorWebApp.csproj ./
# restore dependencies from nuget source if there is any csproj change, otherwies load from cache
RUN dotnet restore "TradingPortal.BlazorWebApp.csproj"
# Copy everything else
COPY . ./
# build from scrach if there is any source code change, otherwies load from cache
RUN dotnet build "TradingPortal.BlazorWebApp.csproj" -c $BUILD_CONFIGURATION --no-restore
# publish but not restore dependencies, as they are already restored 
RUN dotnet publish "TradingPortal.BlazorWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-build --no-restore

# Stage 2: Runtime Image
FROM base AS final
# set container base folder to app
WORKDIR /app
COPY --from=build /app/publish .

# Use HTTPS in Docker
ENV ASPNETCORE_URLS="https://+:443"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Alex!1978

ENTRYPOINT ["dotnet", "TradingPortal.BlazorWebApp.dll"]
