using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Accounts.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountsSupport(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AccountsConfiguration>(sp =>
            configuration.GetSection("Accounts").Get<AccountsConfiguration>());
        services.AddDbContext<AccountContext>((sp, opt) =>
        {
            var cfg = sp.GetRequiredService<AccountsConfiguration>();
            opt.UseSqlite($"Data Source={cfg.DatabasePath}");
        });
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}