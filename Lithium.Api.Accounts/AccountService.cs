using System.Security.Cryptography;
using System.Text;

namespace Lithium.Api.Accounts;

public class AccountService : IAccountService
{
    private readonly AccountContext db;

    public AccountService(AccountContext db)
    {
        this.db = db;
    }

    public async Task AddAccountAsync(NewAccountDto account)
    {
        var accounts = db.Set<Account>();
        accounts.Add(new Account
        {
            Login = account.Login,
            Email = account.Email,
            PasswordChecksum = await ComputeChecksum(account.Password),
        });
        await db.SaveChangesAsync();
    }

    public async Task<bool> ValidatePasswordAsync(string login, string password)
    {
        var accounts = db.Set<Account>();
        var account = accounts.Find(login);
        return account != null &&
            account.PasswordChecksum.SequenceEqual(await ComputeChecksum(password));
    }

    public async Task RemoveAccountAsync(string login)
    {
        var accounts = db.Set<Account>();
        var accountToRemove = accounts.Find(login);
        if (accountToRemove == null)
        {
            throw new KeyNotFoundException($"Account {login} not found");
        }
        accounts.Remove(accountToRemove);
        await db.SaveChangesAsync();
    }

    private async Task<byte[]> ComputeChecksum(string password)
    {
        return SHA1.HashData(Encoding.UTF8.GetBytes(password));
    }
}
