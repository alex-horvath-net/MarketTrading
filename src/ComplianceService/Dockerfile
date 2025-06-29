## --------------------------------------------------
## Stage 1: Base Runtime Image (For Execution)
## --------------------------------------------------
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#ARG APP_UID  # Set the APP_UID argument to the value passed in the docker-compose file
#
## Correct user creation for Debian-based images (default for .NET official images)
#RUN useradd -u ${APP_UID} -m yourBankDockerUser
## Switch to the non-root user for better security
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
#
## Copy only the .csproj file to optimize layer caching
#COPY ["./src/AuditTrailService/AuditTrailService.csproj", "AuditTrailService/"]
#RUN dotnet restore "./AuditTrailService/AuditTrailService.csproj"
#
## --------------------------------------------------
## Stage 3: Build the Service Project
## --------------------------------------------------
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Copy restored project dependencies from restore stage
#COPY --from=restore /src/AuditTrailService/ /src/AuditTrailService/
## Copy the remaining source files into the container
#COPY . .
#WORKDIR "/src/AuditTrailService"
## Build the project and store output in /app/build
#RUN dotnet build "./AuditTrailService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## --------------------------------------------------
## Stage 4: Publish the Service Project
## --------------------------------------------------
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#WORKDIR "/src/AuditTrailService"
#RUN dotnet publish "./AuditTrailService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## --------------------------------------------------
## Stage 5: Final Runtime Image (For Execution)
## --------------------------------------------------
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "AuditTrailService.dll"]
#