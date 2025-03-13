using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TradingPortal.Blazor.Data {
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    }
}

// docker pull mcr.microsoft.com/mssql/server:2022-latest
// docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SA_PASSWORD!123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

// "DefaultConnection": "Server=.,1433;Database=TradingPortal;User Id=sa;Password=SA_PASSWORD!123;TrustServerCertificate=True"

// dotnet tool install --global dotnet-ef
// dotnet tool update --global dotnet-ef

// cd C:\src\github\alex-horvath-net\CleanArchitecture\src\TradingPortal.Blazor
// dotnet add package Microsoft.EntityFrameworkCore.Design

// dotnet ef database update --startup-project TradingPortal.Blazor.csproj --project TradingPortal.Blazor.csproj --context ApplicationDbContext
// dotnet ef migrations list
