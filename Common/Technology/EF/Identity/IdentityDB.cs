using Common.Adapters.Identity.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Common.Technology.EF.Identity;

public class IdentityDB(DbContextOptions<IdentityDB> options) : IdentityDbContext<User>(options) { }
// Add-Migration MigrationName -c IdentityDB -o Technology\IdentityData\Migrations
// Remove-Migration -c IdentityDB
// Update-Database -c IdentityDB 
