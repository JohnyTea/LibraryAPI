namespace Library.API.Data;

public class ApplicationDataBaseContext : DbContext
{
    public ApplicationDataBaseContext(DbContextOptions<ApplicationDataBaseContext> options) : base(options)
    {

    }

    public DbSet<Book> Books { get; set; }

    public DbSet<User> Users { get; set; }
}

