using Microsoft.EntityFrameworkCore;
using TodoApiNet.Models;

namespace TodoApiNet.Contexts
{
    public class TodoApiContext : DbContext
    {
        public TodoApiContext(DbContextOptions<TodoApiContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Todo> Todos { get; set; }
    }
}
