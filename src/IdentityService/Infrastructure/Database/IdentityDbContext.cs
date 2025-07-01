using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Database;

public class IdentityDbContext : IdentityDbContext<IdentityUser> {
    public IdentityDbContext(DbContextOptions options) : base(options) { }
}
