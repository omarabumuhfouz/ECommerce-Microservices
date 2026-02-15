using System.Collections;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Data;
using SharedKernel.Primitives;

namespace SharedKernel.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private Hashtable? _repositories;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public IRepository<T> GetRepository<T>() where T : Entity//, IAggregateRoot
    {
        if (_repositories is null)
        {
            _repositories = new Hashtable();
        }

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>);
            
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T)), 
                _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository<T>)_repositories[type]!;
    }
}