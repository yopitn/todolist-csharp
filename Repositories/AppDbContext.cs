using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Repositories;

public class AppDbContext : DbContext
{
    public DbSet<TodoModel> Todos { get; set; }

    protected AppDbContext()
    {

    }

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
}
