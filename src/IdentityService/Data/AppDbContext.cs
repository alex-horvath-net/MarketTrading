using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class AppDbContext : IdentityDbContext<IdentityUser> {
    public AppDbContext(DbContextOptions options) : base(options) { }
}
