namespace Lithium.Api.Accounts;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Account> Users { get; } = new List<Account>();
}
