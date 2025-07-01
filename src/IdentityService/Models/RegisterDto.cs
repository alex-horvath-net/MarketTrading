using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Models;
public record RegisterDto(string Email, string Password);
public record LoginDto(string Email, string Password);

public class AppDbContext : IdentityDbContext<IdentityUser> {
    public AppDbContext(DbContextOptions options) : base(options) { }
}
