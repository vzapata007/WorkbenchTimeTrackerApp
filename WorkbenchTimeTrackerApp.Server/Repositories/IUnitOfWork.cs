using WorkbenchTimeTrackerApp.Server.Models;

using WorkbenchTimeTrackerApp.Server.Repositories;

/// <summary>
/// Defines the contract for the Unit of Work pattern.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for Person entities.
    /// </summary>
    IRepository<Person> Persons { get; }

    /// <summary>
    /// Gets the repository for TimeEntry entities.
    /// </summary>
    IRepository<TimeEntry> TimeEntries { get; }

    /// <summary>
    /// Gets the repository for WorkTask entities.
    /// </summary>
    IRepository<WorkTask> WorkTasks { get; }

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> CompleteAsync();
}