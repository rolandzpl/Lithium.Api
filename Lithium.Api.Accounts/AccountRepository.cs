using LinqSpecs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Accounts;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext db;

    public AccountRepository(AccountContext db)
    {
        this.db = db;
    }

    public DbSet<Account> Accounts => db.Set<Account>();

    public async Task<DTO?> GetAccountByLoginAsync<DTO>(string login) =>
        await Accounts
            .Where(_ => _.Login == login)
            .ProjectToType<DTO>()
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<DTO>> GetAccountsAsync<DTO>(Specification<Account> filter) =>
        await Accounts
            .Where(filter)
            .ProjectToType<DTO>()
            .ToListAsync();
}
