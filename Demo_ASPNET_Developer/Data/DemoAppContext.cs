namespace Demo_ASPNET_Developer.Data
{
    public class DemoAppContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        //public const string ConnectionString = "";

        public DemoAppContext(DbContextOptions<DemoAppContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product").HasKey(p => p.ID);
                entity.Property(p => p.ID).HasColumnName("ID").ValueGeneratedNever();
                entity.Property(p => p.Name).HasColumnName("Name");
                entity.Property(p => p.Price).HasColumnName("Price");
                entity.Property(p => p.Quantity).HasColumnName("Quantity");
                entity.Property(p => p.Desc).HasColumnName("Desc");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User").HasKey(p => p.ID);
                entity.Property(p => p.ID).HasColumnName("ID").ValueGeneratedNever();
                entity.Property(p => p.Username).HasColumnName("Username");
                entity.Property(p => p.Email).HasColumnName("Email");
                entity.Property(p => p.Password).HasColumnName("Password");
                entity.Property(p => p.UserType).HasColumnName("UserType");
            });
        }
    }
}
