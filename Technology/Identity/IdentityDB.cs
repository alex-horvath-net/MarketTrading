using Adapters.IdentityDtatModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Plugins.Identity;

public class IdentityDB(DbContextOptions<IdentityDB> options) : IdentityDbContext<User>(options) { }
