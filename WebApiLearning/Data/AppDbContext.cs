using Microsoft.EntityFrameworkCore;
using WebApiLearning.Entities;

namespace WebApiLearning.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public  DbSet<User> MstUser { get; set; }
        public DbSet<Employee> EmployeeUser { get; set; }
    }
}
