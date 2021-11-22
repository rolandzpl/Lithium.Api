using LinqSpecs;

namespace Lithium.Api.Accounts;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext db;

    public AccountRepository(AccountContext db)
    {
        this.db = db;
    }

    public IEnumerable<Account> GetAccounts(Specification<Account> filter)
    {
        var set = db.Set<Account>();
        return set.Where(filter).ToList();
    }
}
