﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string> {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
    }
}
