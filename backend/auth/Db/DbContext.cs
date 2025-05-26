using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Db;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens  { get; set; } = null!;
}
