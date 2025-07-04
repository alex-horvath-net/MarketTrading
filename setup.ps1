cd C:\src\github\alex-horvath-net\MarketTrading
cls

#Write-Host "# Ensure cert + key folders & cert exist"
#New-Item -ItemType Directory -Path .\certificates -Force | Out-Null

#Write-Host "# Generating dev certificate…"
#dotnet dev-certs https -ep ./certificates/aspnetapp.pfx -p YourPassword123

#Write-Host "# Trusting dev certificate…"
#dotnet dev-certs https --trust

Write-Host "# Tear down any old docker resources"
docker-compose down

Write-Host "# Clean up packages"
Remove-Item -Recurse -Force .\packages -ErrorAction Ignore

Write-Host "# Clean up NuGets"
dotnet nuget locals all --clear

Write-Host "# Clean up stopped containers"
docker container prune --force

Write-Host "# Clean up unused volumes"
docker volume prune --force

Write-Host "# Clean up unused network"
docker network prune --force

Write-Host "# Clean up build cache"
docker builder prune --all --force

#Write-Host "# Pruning Docker system"
#docker system prune -a --volumes --force
#docker builder prune --all --force

Write-Host "# Start all services"
docker-compose up -d


# cd C:\src\github\alex-horvath-net\MarketTrading\src
# dotnet new webapi -n TradingService
# cd ..
# dotnet sln ../MarketTrading.sln add src/TradingService/TradingService.csproj

