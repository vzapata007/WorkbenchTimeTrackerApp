using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkbenchTimeTrackerApp.Server.Models;

namespace WorkbenchTimeTrackerApp.Server.Repositories
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Asynchronously retrieves all entities of type T.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a result containing a collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves all entities of type T with filtering and including related entities.
        /// </summary>
        /// <param name="filter">Optional filter to apply to the query.</param>
        /// <param name="includes">Optional related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing a collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes);



        /// <summary>
        /// Asynchronously retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the entity or null if not found.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id);
    }
}