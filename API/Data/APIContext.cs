using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApiContext : DbContext {

    private readonly IConfiguration _configuration;

    public ApiContext(IConfiguration configuration) {
        this._configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlite(connectionString);
    }

    public DbSet<User> Users { get; set; }
}