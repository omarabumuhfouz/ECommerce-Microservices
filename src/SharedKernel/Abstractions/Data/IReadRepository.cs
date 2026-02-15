using System.Linq.Expressions;
using Ardalis.Specification; // Using the Ardalis interface
using SharedKernel.Primitives;

namespace SharedKernel.Abstractions.Data;

public interface IReadRepository<T> where T :  Entity
{
    Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<List<T>> GetListAsync(CancellationToken cancellationToken = default);
    Task<List<TResult>> GetListAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}