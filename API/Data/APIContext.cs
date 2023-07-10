using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class APIContext : DbContext {

    private readonly IConfiguration configuration;

    public APIContext(IConfiguration configuration) {
        this.configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlite(connectionString);
    }

    public DbSet<User> Users { get; set; }
}