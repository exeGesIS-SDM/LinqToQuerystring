namespace LinqToQueryString.EntityFrameworkCore.Tests
{
    using LinqToQueryString.Tests;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class TestDbContext : DbContext
    {
        public DbSet<ConcreteClass> ConcreteClasses { get; set; }

        public DbSet<ComplexClass> ComplexClasses { get; set; }

        public DbSet<EdgeCaseClass> EdgeCaseClasses { get; set; }

        public DbSet<NullableClass> NullableValues { get; set; }

        public DbSet<NullableContainer> NullableContainers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TestDb.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConcreteClass>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Value).IsRequired();
                entity.Property(x => x.Cost).IsRequired();
                entity.Ignore(x => x.StringCollection);
            });

            modelBuilder.Entity<NullableClass>(entity =>
            {
                entity.Ignore(x => x.NullableInts);
            });

            modelBuilder.Entity<ComplexClass>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Title);
                entity.Ignore(x => x.StringCollection);
                entity.Ignore(x => x.IntCollection);
                //entity.HasOne(x => x.Concrete).WithOne();
                entity.HasOne(x => x.Concrete);
                entity.HasMany(x => x.ConcreteCollection).WithOne();
            });
        }
    }
}
