using Microsoft.EntityFrameworkCore;

namespace TodoApp;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; } = null!;
}