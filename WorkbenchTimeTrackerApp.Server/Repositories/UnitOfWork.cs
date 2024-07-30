using WorkbenchTimeTrackerApp.Server.Data;
using WorkbenchTimeTrackerApp.Server.Models;
using WorkbenchTimeTrackerApp.Server.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace WorkbenchTimeTrackerApp.Server.Repositories
{
    /// <summary>
    /// Implements the Unit of Work pattern, coordinating the repositories and managing transactions 
    /// (in this case, just one generic repository for each entity type).
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Person> _persons;
        private IRepository<TimeEntry> _timeEntries;
        private IRepository<WorkTask> _workTasks;
        private IDbContextTransaction _transaction;


        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the repository for Person entities.
        /// </summary>
        public IRepository<Person> Persons => _persons ??= new Repository<Person>(_context);

        /// <summary>
        /// Gets the repository for TimeEntry entities.
        /// </summary>
        public IRepository<TimeEntry> TimeEntries => _timeEntries ??= new Repository<TimeEntry>(_context);

        /// <summary>
        /// Gets the repository for WorkTask entities.
        /// </summary>
        public IRepository<WorkTask> WorkTasks => _workTasks ??= new Repository<WorkTask>(_context);

        /// <summary>
        /// Saves all changes made in the context to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<int> CompleteAsync()
        {
            try
            {
                // Begin a transaction if none is started
                if (_transaction == null)
                {
                    _transaction = await _context.Database.BeginTransactionAsync();
                }

                // Save changes in the context
                var result = await _context.SaveChangesAsync();

                // Commit the transaction
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    _transaction.Dispose();
                    _transaction = null;
                }

                return result;
            }
            catch (Exception)
            {
                // Rollback the transaction in case of an error
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    _transaction.Dispose();
                    _transaction = null;
                }

                throw; // Rethrow the exception to be handled by the caller
            }
        }

        /// <summary>
        /// Disposes of the UnitOfWork and the underlying context.
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}