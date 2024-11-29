using Microsoft.EntityFrameworkCore;

namespace TodoMinimalAPI
{
    public class TodoDB : DbContext
    {
        public TodoDB(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
