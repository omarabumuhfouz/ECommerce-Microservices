using Ardalis.Specification;
using SharedKernel.Primitives;

namespace SharedKernel.Abstractions.Data;

public interface IRepository<T> : IReadRepository<T> where T : Entity
{
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
    Task<int> DeleteBySpecAsync(ISpecification<T> spec, CancellationToken ct = default);
}