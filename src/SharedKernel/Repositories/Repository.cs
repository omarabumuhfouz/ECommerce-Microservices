using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Data;
using SharedKernel.Primitives;

namespace SharedKernel.Repositories;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }



public async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(ct);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, ct);
    }

    public async Task<List<T>> GetListAsync(CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync(ct);
    }

    public async Task<List<T>> GetListAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).ToListAsync(ct);
    }

    public async Task<List<TResult>> GetListAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).ToListAsync(ct);
    }

    // --- EXISTENCE & AGGREGATION ---

    public async Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).AnyAsync(ct);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate, ct);
    }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().CountAsync(ct);
    }

    public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).CountAsync(ct);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().CountAsync(predicate, ct);
    }

    // --- COMMANDS (WRITE) ---

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await _dbContext.Set<T>().AddAsync(entity, ct);

    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);

    public async Task<int> DeleteBySpecAsync(ISpecification<T> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).ExecuteDeleteAsync(ct);
    }

    // --- PRIVATE EVALUATORS ---

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator.Default.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator.Default.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);
    }
}