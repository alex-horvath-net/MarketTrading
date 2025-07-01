cd C:\src\github\alex-horvath-net\MarketTrading
cls

# 1) Ensure cert + key folders & cert exist
New-Item -ItemType Directory -Path .\certificates -Force | Out-Null
dotnet dev-certs https -ep ./certificates/aspnetapp.pfx -p YourPassword123
dotnet dev-certs https --trust

# 2) Close Visual Studio (important!)
Stop-Process -Name devenv -Force -ErrorAction Ignore

# 3) Tear down any old Docker resources
docker-compose down

# 4) Start SQL only so we can migrate
docker-compose up -d sql

# 5) Wait for SQL to be ready
Write-Host "Waiting for SQL to come online…" -NoNewline
while (-not (Test-NetConnection -ComputerName localhost -Port 1433 -WarningAction SilentlyContinue).TcpTestSucceeded) {
    Write-Host "." -NoNewline
    Start-Sleep 1
}
Write-Host " OK"

# 6) Apply EF Core migrations against that SQL
Push-Location .\src\IdentityService\
dotnet ef database update
Pop-Location

# 7) Clean up Docker caches
Remove-Item -Recurse -Force -ErrorAction Ignore .\packages
dotnet nuget locals all --clear
docker system prune -a --volumes --force
docker builder prune --all --force

# 8) Build & start the rest of your services
docker-compose up -d --build tradingportal identity

# 9) Tail their logs
docker-compose logs -f tradingportal identity
