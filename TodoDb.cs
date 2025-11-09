using Microsoft.EntityFrameworkCore;

 public class Db : DbContext
{
    public Db(DbContextOptions<Db> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}

 public class AppDbContext : DbContext
 {
     public DbSet<TodoItem> Todos { get; set; }

     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
 }