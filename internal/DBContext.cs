using Microsoft.EntityFrameworkCore;

namespace TodoApp;

public class Db(DbContextOptions<Db> options) : DbContext(options) {
    public DbSet<Todo> Todos { get; set; } = null!;
}