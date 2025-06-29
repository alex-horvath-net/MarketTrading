## --------------------------------------------------
## Stage 1: Base Runtime Image (For Execution)
## --------------------------------------------------
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#ARG APP_UID
#RUN useradd -u ${APP_UID} -m yourBankDockerUser
#USER yourBankDockerUser
#WORKDIR /app
#EXPOSE 80
#
## --------------------------------------------------
## Stage 2: Restore Dependencies
## --------------------------------------------------
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["./src/PricingEngineService/PricingEngineService.csproj", "PricingEngineService/"]
#RUN dotnet restore "./PricingEngineService/PricingEngineService.csproj"
#
## --------------------------------------------------
## Stage 3: Build
## --------------------------------------------------
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY --from=restore /src/PricingEngineService/ /src/PricingEngineService/
#COPY . .
#WORKDIR "/src/PricingEngineService"
#RUN dotnet build "./PricingEngineService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## --------------------------------------------------
## Stage 4: Publish
## --------------------------------------------------
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#WORKDIR "/src/PricingEngineService"
#RUN dotnet publish "./PricingEngineService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## --------------------------------------------------
## Stage 5: Final Runtime Image
## --------------------------------------------------
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "PricingEngineService.dll"]
#