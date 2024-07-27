using Microsoft.EntityFrameworkCore;
using WorkbenchTimeTrackerApp.Server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WorkbenchTimeTrackerApp.Server.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Ensure database is created and migrate
            await context.Database.MigrateAsync();

            // Seed data if not already present
            if (!context.People.Any())
            {
                context.People.AddRange(
                    new Person { FullName = "Victor Zapata" },
                    new Person { FullName = "Valentina Zapata" },
                    new Person { FullName = "Isidora Zapata" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.WorkTasks.Any())
            {
                var people = context.People.ToList(); // Retrieve people to assign tasks
                context.WorkTasks.AddRange(
                    new WorkTask { Name = "Task 1", Description = "Description for Task 1" },
                    new WorkTask { Name = "Task 2", Description = "Description for Task 2" },
                    new WorkTask { Name = "Task 3", Description = "Description for Task 3" }
                );
                await context.SaveChangesAsync();

                var tasks = context.WorkTasks.ToList(); // Retrieve tasks after saving

                if (people.Count > 0 && tasks.Count > 0)
                {
                    context.TimeEntries.AddRange(
                        new TimeEntry { EntryDateTime = DateTime.Now, WorkTaskId = tasks[0].Id, PersonId = people[0].Id },
                        new TimeEntry { EntryDateTime = DateTime.Now, WorkTaskId = tasks[1].Id, PersonId = people[1].Id },
                        new TimeEntry { EntryDateTime = DateTime.Now, WorkTaskId = tasks[2].Id, PersonId = people[2].Id }
                    );
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}