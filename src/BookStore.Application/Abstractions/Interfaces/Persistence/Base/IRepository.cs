using System.Data;
using System.Linq.Expressions;
using BookStore.SharedKernel.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Abstractions.Interfaces.Persistence.Base;

public interface IRepository<TEntity, in TEntityId>
                    where TEntity : Entity<TEntityId>
                    where TEntityId : ValueObject
{
    IQueryable<TEntity> GetAllAsync(bool trackChanges = false);
    IQueryable<TEntity> GetFilteredAsync(
                                    Expression<Func<TEntity, bool>> predicate,
                                    bool trackChanges = false);

    IQueryable<TEntity> GetFilteredWithReferencesAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool trackChanges = false,
        params string[] includes);




    Task<TEntity> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);



    public IQueryable<TEntity> GetListWithReferencesAndOrderBysAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeString = null,
        bool disableTracking = true);


    Task<TEntity> FirstOrDefaultWithReferencesAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params string[] includes);


    Task<TEntity> FirstOrDefaultAsync(
                        Expression<Func<TEntity, bool>> predicate,
                        CancellationToken cancellationToken = default);



    Task<TEntity> CreateAsync(TEntity entity);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);
    void UpdateRange(List<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveById(TEntityId id);
    void RemoveRange(List<TEntity> entities);

    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    Task<bool> AnyAsync();
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    void ExecuteCommandStoredProcedure(string storedProcedure, params SqlParameter[] parameters);

    public DataTable SelectDataByStoredProcedure(string storedProcedure, SqlParameter[] parameters);
    public EntityState TrackEntityState(TEntity entity);


    void Attach(TEntity entity);
    void AttachRange(IEnumerable<TEntity> entities);

    void DetachEntity(TEntity entity);
    // Debugging and Conflict Handling (NEW)
    /// <summary>
    /// Logs all entities currently tracked by the DbContext. Useful for debugging.
    /// </summary>
    void LogTrackedEntities();

    /// <summary>
    /// Resolves conflicts by detaching an entity if it causes tracking issues.
    /// </summary>
    void ResolveTrackingConflict(TEntity entity);
    Task<List<TResponse>> SelectData<TResponse>(FormattableString sql);



    public Task<List<TResponse>> SelectSqlQueryListAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null);
    public Task<TResponse> SelectSqlQueryFirstOrDefaultAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null);
    public Task ExecuteSqlQueryCommandAsync(string sqlQuery, Dictionary<string, object>? parameters = null);

    public Task<List<TResponse>> SelectStoredProcedureListAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null);
    public Task<TResponse> SelectStoredProcedureFirstOrDefaultAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null);

    public Task ExecuteStoredProcedureAsync(string sqlQuery, Dictionary<string, object>? parameters = null);

}
