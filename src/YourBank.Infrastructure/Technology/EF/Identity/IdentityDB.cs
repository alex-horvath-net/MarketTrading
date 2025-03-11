using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Adapters.Identity.Data.Model;

namespace Infrastructure.Technology.EF.Identity;

public class IdentityDB(DbContextOptions<IdentityDB> options) : IdentityDbContext<User>(options) { }
// Add-Migration MigrationName -c IdentityDB -o Technology\IdentityData\Migrations
// Remove-Migration -c IdentityDB
// Update-Database -c IdentityDB 
