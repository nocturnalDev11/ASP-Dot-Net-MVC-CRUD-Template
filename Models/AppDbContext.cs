using Microsoft.EntityFrameworkCore;
using ASP_Dot_Net_MVC_CRUD_Template.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Item> Items { get; set; }
}
