FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN mkdir -p /certificates && chmod 700 /certificates
RUN mkdir -p /keys && chmod 777 /keys
WORKDIR /app
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj", "src/TradingPortal.BlazorServer/"]
RUN dotnet restore "src/TradingPortal.BlazorServer/TradingPortal.BlazorServer.csproj"
COPY . .
WORKDIR "/src/src/TradingPortal.BlazorServer"
RUN dotnet build "TradingPortal.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TradingPortal.BlazorServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY certificates/aspnetapp.pfx /certificates/aspnetapp.pfx

ENV ASPNETCORE_URLS="https://+:443"
ENTRYPOINT ["dotnet", "TradingPortal.BlazorServer.dll"]
