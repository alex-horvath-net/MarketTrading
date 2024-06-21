using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Technology.Data.Identity;

public class IdentityDB(DbContextOptions<IdentityDB> options) : IdentityDbContext<User>(options) { }
