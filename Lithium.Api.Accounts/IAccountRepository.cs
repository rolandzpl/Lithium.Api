using LinqSpecs;

namespace Lithium.Api.Accounts;

public interface IAccountRepository
{
    Task<DTO?> GetAccountByLoginAsync<DTO>(string login);
    
    Task<IEnumerable<DTO>> GetAccountsAsync<DTO>(Specification<Account> filter);
}
