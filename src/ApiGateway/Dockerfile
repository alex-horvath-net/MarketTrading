# --------------------------------------------------
# Stage 1: Base Runtime Image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=1001
RUN useradd -u ${APP_UID} -m yourBankDockerUser
USER yourBankDockerUser
WORKDIR /app
EXPOSE 443  # Expose HTTPS port

# --------------------------------------------------
# Stage 2: Restore Dependencies
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/ApiGateway/ApiGateway.csproj", "ApiGateway/"]
RUN dotnet restore "./ApiGateway/ApiGateway.csproj"

# --------------------------------------------------
# Stage 3: Build the ApiGateway Project
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY --from=restore /src/ApiGateway/ /src/ApiGateway/
COPY . .
WORKDIR "/src/ApiGateway"
RUN dotnet build "./ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------------------------------
# Stage 4: Publish the Application
# --------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/ApiGateway"
RUN dotnet publish "./ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --------------------------------------------------
# Stage 5: Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app

# Copy published app
COPY --from=publish /app/publish .

# ❗ Required for HTTPS binding
ENV ASPNETCORE_URLS="https://+:443"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=""

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
