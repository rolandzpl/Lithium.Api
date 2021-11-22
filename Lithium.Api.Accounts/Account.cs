namespace Lithium.Api.Accounts;

public class Account
{
    public string Login { get; set; }
    public string Email { get; set; }
    public byte[] PasswordChecksum { get; set; }

    public List<Group> Groups { get; } = new List<Group>();
}
