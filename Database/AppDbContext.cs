using Microsoft.EntityFrameworkCore;

class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source=shoppingcart.db");
    }

    /// <summary>
    /// When creating db model, add hidden 'ProductType' column to tell rows apart. 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasDiscriminator<string>("ProductType")
            .HasValue<Book>("Book")
            .HasValue<Electronics>("Electronics")
            .HasValue<DryGood>("DryGood")
            .HasValue<Furniture>("Furniture");
    }
}