# --------------------------------------------------
# Stage 1: Base Runtime Image (For Execution)
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID
RUN useradd -u ${APP_UID} -m yourBankDockerUser
USER yourBankDockerUser
WORKDIR /app
EXPOSE 443

# --------------------------------------------------
# Stage 2: Restore Dependencies
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/IdentityService/IdentityService.csproj", "IdentityService/"]
RUN dotnet restore "./IdentityService/IdentityService.csproj"

# --------------------------------------------------
# Stage 3: Build the Service Project
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY --from=restore /src/IdentityService/ /src/IdentityService/
COPY . .
WORKDIR "/src/IdentityService"
RUN dotnet build "./IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --------------------------------------------------
# Stage 4: Publish the Service Project
# --------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/IdentityService"
RUN dotnet publish "./IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --------------------------------------------------
# Stage 5: Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="https://+:443"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Alex!1978