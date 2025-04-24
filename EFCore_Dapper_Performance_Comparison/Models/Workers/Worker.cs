using EFCore_Dapper_Performance_Comparison.Models.Transactions;

namespace EFCore_Dapper_Performance_Comparison.Models.Workers;
public class Worker
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public WorkerRole Role { get; set; }

    public ICollection<Transaction>? Transactions { get; private set; }

    public Worker(
        string firstName,
        string lastName,
        WorkerRole role,
        Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }

    public Worker() { }
}
