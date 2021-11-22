namespace Lithium.Api.Accounts;

public class Account
{
    public string Login { get; init; }

    public string Email { get; init; }

    public byte[] PasswordChecksum { get; init; }

    public List<Group> Groups { get; } = new List<Group>();
}
