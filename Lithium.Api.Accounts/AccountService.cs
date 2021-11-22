using System.Security.Cryptography;
using System.Text;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lithium.Api.Accounts;

public class AccountService : IAccountService
{
    private readonly AccountContext db;

    public AccountService(AccountContext db)
    {
        this.db = db;
    }

    public DbSet<Account> Accounts => db.Set<Account>();

    public async Task AddAccountAsync(NewAccountDto account)
    {
        Accounts.Add(new Account
        {
            Login = account.Login,
            Email = account.Email,
            PasswordChecksum = await ComputeChecksum(account.Password),
        });
        await db.SaveChangesAsync();
    }

    public async Task<bool> ValidatePasswordAsync(string login, string password)
    {
        var passwordChecksum = (await Accounts
            .Where(_ => _.Login == login)
            .ProjectToType<UserPasswordChecksumOnly>()
            .SingleOrDefaultAsync())
            ?.PasswordChecksum;
        return passwordChecksum != null && passwordChecksum.SequenceEqual(await ComputeChecksum(password));
    }

    class UserPasswordChecksumOnly
    {
        public byte[] PasswordChecksum { get; init; }
    }

    public async Task RemoveAccountAsync(string login)
    {
        var accountToRemove = Accounts.Find(login);
        if (accountToRemove == null)
        {
            throw new KeyNotFoundException($"Account {login} not found");
        }
        Accounts.Remove(accountToRemove);
        await db.SaveChangesAsync();
    }

    private async Task<byte[]> ComputeChecksum(string password) =>
        await Task.Run(() => SHA1.HashData(Encoding.UTF8.GetBytes(password)));
}
