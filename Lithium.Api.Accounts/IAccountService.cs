namespace Lithium.Api.Accounts;

public interface IAccountService
{
    Task AddAccountAsync(NewAccountDto account);
    Task<bool> ValidatePasswordAsync(string login, string password);
    Task RemoveAccountAsync(string login);
}
