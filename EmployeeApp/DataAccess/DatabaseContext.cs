using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<EmployeeModel> Employees { get; set; }

        public  DbSet<LocationModel> Locations { get; set; }

        public DbSet<TaskModel> Tasks { get; set; }

    }
}
