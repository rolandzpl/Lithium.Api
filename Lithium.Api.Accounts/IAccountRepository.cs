using LinqSpecs;

namespace Lithium.Api.Accounts;

public interface IAccountRepository
{
    IEnumerable<Account> GetAccounts(Specification<Account> filter);
}
