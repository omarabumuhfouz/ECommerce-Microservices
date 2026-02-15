using SharedKernel.Abstractions.Data;
using SharedKernel.Primitives; 

namespace SharedKernel.Abstractions;

public interface IUnitOfWork
{
    // Saves changes to the database (Transaction Commit)
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Gets the repository for a specific Aggregate Root
    IRepository<T> GetRepository<T>() where T : Entity;//, IAggregateRoot;
}