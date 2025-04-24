namespace EFCore_Dapper_Performance_Comparison.Common.IUnitOfWork;
public interface IUnitOfWork
{
    Task CommitChangesAsync();
}
