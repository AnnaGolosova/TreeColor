using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace TreeColor.Server.Abstract
{
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncRepository<T>
    {
        /// <summary>
        /// Adds a new item asynchronously
        /// </summary>
        /// <param name="item">entity</param>
        /// <returns></returns>
        Task<T> AddAsync(T item);

        /// <summary>
        /// Add a new items asynchronously
        /// </summary>
        /// <param name="items">Entities</param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<T> items);

        /// <summary>
        /// Updates item
        /// </summary>
        /// <param name="item">item to update</param>
        /// <returns></returns>
        Task<T> UpdateAsync(T item);

        /// <summary>
        /// Updates list
        /// </summary>
        /// <param name="items">items to update</param>
        /// <returns></returns>
        Task UpdateRangeAsync(IEnumerable<T> items);

        /// <summary>
        /// Returns item by id asynchronously
        /// </summary>
        /// <param name="id">item id</param>
        /// <returns></returns>
        Task<T> GetAsync(params object[] id);

        /// <summary>
        /// Returns items by expression asynchronously
        /// </summary>
        /// <param name="condition">condition to get items</param>
        /// <returns></returns>
        IEnumerable<T> Get(Expression<Func<T, bool>> condition = null);

        /// <summary>
        /// Removes item asynchronously
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns></returns>
        Task RemoveAsync(T item);

        /// <summary>
        /// Remove items asynchronously
        /// </summary>
        /// <param name="items">Items to remove</param>
        /// <returns></returns>
        Task RemoveRangeAsync(IEnumerable<T> items);

        /// <summary>
        /// Removes item by id asynchronously
        /// </summary>
        /// <param name="id">item id</param>
        /// <returns></returns>
        Task RemoveAsync(object id);
    }
}