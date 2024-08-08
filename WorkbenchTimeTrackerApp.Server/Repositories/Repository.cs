using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkbenchTimeTrackerApp.Server.Data;

namespace WorkbenchTimeTrackerApp.Server.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Asynchronously retrieves all entities of type T.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a result containing a collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions specifically
                throw new InvalidOperationException("An error occurred while retrieving all entities.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An unexpected error occurred while retrieving all entities.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves all entities of type T with optional filtering and includes related entities.
        /// </summary>
        /// <param name="filter">Optional filter expression to apply.</param>
        /// <param name="includes">Optional related entities to include.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing a collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includes != null && includes.Length > 0)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query.ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions specifically
                throw new InvalidOperationException("An error occurred while retrieving filtered entities.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An unexpected error occurred while retrieving filtered entities.", ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the entity or null if not found.</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where the find operation fails
                throw new KeyNotFoundException($"Entity with ID {id} was not found.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception($"An unexpected error occurred while retrieving the entity with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity)); // Ensure the entity is not null
            }

            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions specifically
                throw new InvalidOperationException("An error occurred while adding the entity.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An unexpected error occurred while adding the entity.", ex);
            }
        }

        /// <summary>
        /// Asynchronously updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity)); // Ensure the entity is not null
            }

            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency issues
                throw new InvalidOperationException("An error occurred due to a concurrency conflict while updating the entity.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions specifically
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }

        /// <summary>
        /// Asynchronously deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Entity with ID {id} was not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions specifically
                throw new InvalidOperationException($"An error occurred while deleting the entity with ID {id}.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception($"An unexpected error occurred while deleting the entity with ID {id}.", ex);
            }
        }
    }
}