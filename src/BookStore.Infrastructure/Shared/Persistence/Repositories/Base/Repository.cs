using System.Data;
using System.Linq.Expressions;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.SharedKernel.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SqlToObjectify;

#pragma warning disable CS8604
#pragma warning disable CS8603

namespace BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

public class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : ValueObject
{
    protected BookStoreAppContext BookStoreAppContext;
    private readonly DbSet<TEntity> _db;

    public Repository(BookStoreAppContext spardaDbContext)
    {
        BookStoreAppContext = spardaDbContext;
        _db = BookStoreAppContext.Set<TEntity>();

    }

    public IQueryable<TEntity> GetAllAsync(bool trackChanges = false) => !trackChanges ? _db.AsNoTracking() : _db;


    public IQueryable<TEntity> GetFilteredAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool trackChanges = false)
    {
        var query = !trackChanges
            ? _db.AsNoTracking()
            : _db;

        return query.Where(predicate);

    }

    public IQueryable<TEntity> GetFilteredWithReferencesAsync(
                    Expression<Func<TEntity, bool>> predicate,
                    bool trackChanges = false,
                    params string[] includes)
    {
        var query = !trackChanges
            ? _db.AsNoTracking()
            : _db;

        if (includes.Any())
        {
            query = Include(includes).AsQueryable();
        }

        return query.Where(predicate);

    }

    public IQueryable<TEntity> GetListWithReferencesAndOrderBysAsync(
                                                            Expression<Func<TEntity, bool>>? predicate = null,
                                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                            string? includeString = null,
                                                            bool disableTracking = true)
    {
        IQueryable<TEntity> query = _db;
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString))
        {
            query = query.Include(includeString);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<TEntity> GetByIdAsync(
                                TEntityId id,
                                CancellationToken cancellationToken = default)
    {

        return await _db.FindAsync(id, cancellationToken);
    }


    public async Task<TEntity> FirstOrDefaultWithReferencesAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        IQueryable<TEntity>? query = _db;

        if (includes.Any())
        {
            query = Include(includes).AsQueryable();
        }

        return await query.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }




    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _db.FirstOrDefaultAsync(predicate, cancellationToken);
    }



    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        EntityEntry<TEntity>? entityForCreation = await _db.AddAsync(entity);
        return entityForCreation.Entity;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _db.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _db.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _db.Update(entity);
    }

    public void UpdateRange(List<TEntity> entities)
    {
        _db.UpdateRange(entities);
    }

    /// <summary>
    /// delete the entity
    /// </summary>
    /// <param name="entity"></param>
    public void Remove(TEntity entity)
    {
        _db.Remove(entity);
    }

    /// <summary>
    /// first find the entity then delete it
    /// </summary>
    /// <param name="id"></param>
    public void RemoveById(TEntityId id)
    {
        var entityToDelete = _db.Find(id);
        Remove(entityToDelete);
    }

    public void RemoveRange(List<TEntity> entities)
    {
        _db.RemoveRange(entities);
    }

    public async Task<int> CountAsync()
    {
        var result = await _db.CountAsync();
        return result;
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await _db.CountAsync(predicate);
        return result;
    }


    public async Task<bool> AnyAsync()
    {
        var result = await _db.AnyAsync();
        return result;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await _db.AnyAsync(predicate);
        return result;
    }
    public EntityState TrackEntityState(TEntity entity)
    {
        var result = _db.Entry(entity).State;

        return result;

    }

    public void Attach(TEntity entity)
    {
        if (entity != null)
        {
            _db.Attach(entity);
        }
    }

    public void AttachRange(IEnumerable<TEntity> entities)
    {
        if (entities != null && entities.Any())
        {
            _db.AttachRange(entities);
        }
    }

    public void DetachEntity(TEntity entity)
    {
       var entry = _db.Entry(entity);
       if (entry != null && entry.State != EntityState.Detached)
       {
            entry.State = EntityState.Detached;
       }
    }
    public void LogTrackedEntities()
    {   
        var trackedEntities = BookStoreAppContext.ChangeTracker.Entries()
            .Select(e => new
            {
                EntityType = e.Entity.GetType().Name,
                State = e.State,
                KeyValues = e.Properties
                    .Where(p => p.Metadata.IsPrimaryKey())
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)
            })
            .ToList();

        foreach (var entity in trackedEntities)
        {
            Console.WriteLine($"Entity: {entity.EntityType}, State: {entity.State}");
            foreach (KeyValuePair<string, object?> keyValue in entity.KeyValues)
            {
                Console.WriteLine($"  {keyValue.Key}: {keyValue.Value}");
            }
        }
    }

    public void ResolveTrackingConflict(TEntity entity)
    {
        var entry = BookStoreAppContext.Entry(entity);

        if (entry.State == EntityState.Detached)
            return;

        entry.State = EntityState.Detached;
    }



    #region Using SQL Raw

    public void ExecuteCommandStoredProcedure(string storedProcedure, SqlParameter[]? parameters = null)
    {
        var connection = BookStoreAppContext
            .Database
            .GetDbConnection();

        var con = new SqlConnection(connection.ConnectionString);
        var sqlCmd = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = storedProcedure,
            Connection = con
        };


        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        //if (parameters is not null && parameters.Length > 0
        if (parameters is { Length: > 0 })

        {
            sqlCmd.Parameters.AddRange(parameters);
        }

        con.Open();
        sqlCmd.ExecuteNonQuery();
        con.Close();
    }

    public DataTable SelectDataByStoredProcedure(string storedProcedure, SqlParameter[]? parameters = null)
    {
        var connection = BookStoreAppContext.Database.GetDbConnection();
        var con = new SqlConnection(connection.ConnectionString);
        var dt = new DataTable();

        var sqlCmd = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = storedProcedure,
            Connection = con
        };

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (parameters is not null && parameters.Length > 0)
        {
            foreach (var p in parameters)
            {
                sqlCmd.Parameters.Add(p);
            }
        }
        var da = new SqlDataAdapter(sqlCmd);


        da.Fill(dt);

        return dt;
    }

    public async Task<List<TResponse>> SelectSqlQueryListAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null)
    {
        return await BookStoreAppContext.SelectSqlQueryListAsync<TResponse>(sqlQuery, parameters);
    }

    public async Task<TResponse> SelectSqlQueryFirstOrDefaultAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null)
    {
        return await BookStoreAppContext.SelectSqlQueryFirstOrDefaultAsync<TResponse>(sqlQuery, parameters);
    }

    public async Task ExecuteSqlQueryCommandAsync(string sqlQuery, Dictionary<string, object>? parameters = null)
    {
        await BookStoreAppContext.ExecuteSqlQueryCommandAsync(sqlQuery, parameters);
    }

    public async Task<List<TResponse>> SelectStoredProcedureListAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null)
    {
        return await BookStoreAppContext.SelectStoredProcedureListAsync<TResponse>(sqlQuery, parameters);
    }

    public async Task<TResponse> SelectStoredProcedureFirstOrDefaultAsync<TResponse>(string sqlQuery, Dictionary<string, object>? parameters = null)
    {
        return await BookStoreAppContext.SelectStoredProcedureFirstOrDefaultAsync<TResponse>(sqlQuery, parameters);
    }

    public async Task ExecuteStoredProcedureAsync(string sqlQuery, Dictionary<string, object>? parameters = null)
    {
        await BookStoreAppContext.ExecuteStoredProcedureAsync(sqlQuery, parameters);
    }
    #endregion




    private IEnumerable<TEntity> Include(params string[] includes)
    {
        IEnumerable<TEntity> query = null;
        foreach (var include in includes)
        {
            query = _db.Include(include);
        }
        return query ?? _db;
    }

}
