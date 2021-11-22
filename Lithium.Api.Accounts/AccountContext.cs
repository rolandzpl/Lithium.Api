using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lithium.Api.Accounts;

public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasKey(_ => _.Login);
        modelBuilder.Entity<Account>().HasMany(_ => _.Groups).WithMany(_ => _.Users);
        modelBuilder.Entity<Group>().HasKey(_ => _.Id);
        modelBuilder.Entity<Group>().HasMany(_ => _.Users).WithMany(_ => _.Groups);
    }
}

public class AccountContextFactory : IDesignTimeDbContextFactory<AccountContext>
{
    public AccountContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AccountContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");
        return new AccountContext(optionsBuilder.Options);
    }
}