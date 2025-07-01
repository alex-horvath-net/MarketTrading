cd .\src\IdentityService
dotnet ef migrations add AddIdentityTables -c IdentityDbContext -o Infrastructure\Database\Migrations