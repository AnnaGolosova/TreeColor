using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using TreeColor.Server.Abstract;

namespace TreeColor.Server.Data.Repositories
{
    ///// <inheritdoc/>
    //public class AsyncRepository<T> : IAsyncRepository<T> where T : class
    //{
    //    private readonly AuthDataContext _dataContext;

    //    /// <inheritdoc/>
    //    public AsyncRepository(AuthDataContext eventDbContext)
    //    {
    //        _dataContext = eventDbContext;
    //    }

    //    /// <inheritdoc/>
    //    public async Task<T> AddAsync(T item)
    //    {
    //        var added = await _dataContext.AddAsync<T>(item);
    //        await _dataContext.SaveChangesAsync();

    //        return added.Entity;
    //    }

    //    /// <inheritdoc/>
    //    public async Task AddRangeAsync(IEnumerable<T> items)
    //    {
    //        await _dataContext.AddRangeAsync(items);
    //        await _dataContext.SaveChangesAsync();
    //    }

    //    /// <inheritdoc/>
    //    public IEnumerable<T> Get(Expression<Func<T, bool>> condition = null)
    //    {
    //        IEnumerable<T> items;
    //        if (condition != null)
    //            items = _dataContext.Set<T>().AsNoTracking().Where(condition);
    //        else
    //            items = _dataContext.Set<T>().AsNoTracking().AsEnumerable();

    //        return items;
    //    }

    //    /// <inheritdoc/>
    //    public async Task<T> GetAsync(params object[] id)
    //    {
    //        T item = await _dataContext.FindAsync<T>(id);

    //        return item;
    //    }

    //    /// <inheritdoc/>
    //    public async Task RemoveAsync(T item)
    //    {
    //        _dataContext.Remove(item);
    //        await _dataContext.SaveChangesAsync();
    //    }

    //    /// <inheritdoc/>
    //    public async Task RemoveRangeAsync(IEnumerable<T> items)
    //    {
    //        _dataContext.RemoveRange(items);
    //        await _dataContext.SaveChangesAsync();
    //    }

    //    /// <inheritdoc/>
    //    public async Task RemoveAsync(object id)
    //    {
    //        T item = await _dataContext.FindAsync<T>(id);

    //        if (item != null)
    //        {
    //            _dataContext.Remove<T>(item);
    //            await _dataContext.SaveChangesAsync();
    //        }
    //    }

    //    /// <inheritdoc/>
    //    public async Task<T> UpdateAsync(T item)
    //    {
    //        var updated = _dataContext.Update<T>(item);
    //        await _dataContext.SaveChangesAsync();

    //        return updated.Entity;
    //    }

    //    /// <inheritdoc/>
    //    public async Task UpdateRangeAsync(IEnumerable<T> items)
    //    {
    //        _dataContext.UpdateRange(items);
    //        await _dataContext.SaveChangesAsync();
    //    }
    //}
}