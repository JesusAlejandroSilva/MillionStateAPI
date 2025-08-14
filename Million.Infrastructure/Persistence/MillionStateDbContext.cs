using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Million.Domain.Entities;

namespace Million.Infrastructure.Persistence
{
    /// <summary>DbContext contra la base MillionStateDB existente</summary>
    public class MillionStateDbContext : DbContext
    {
        public MillionStateDbContext(DbContextOptions<MillionStateDbContext> options) : base(options) { }

        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
        public DbSet<PropertyTrace> PropertyTraces => Set<PropertyTrace>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                v => v.ToDateTime(TimeOnly.MinValue),
                v => DateOnly.FromDateTime(v));

            modelBuilder.Entity<Owner>(b =>
            {
                b.ToTable("Owner", "dbo");
                b.HasKey(x => x.IdOwner).HasName("PK_Owner");
                b.Property(x => x.Name).IsRequired().HasMaxLength(150);
                b.Property(x => x.Address).HasMaxLength(250);
                b.Property(x => x.Photo).HasMaxLength(500);
            });

            modelBuilder.Entity<Property>(b =>
            {
                b.ToTable("Property", "dbo");
                b.HasKey(x => x.IdProperty).HasName("PK_Property");
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.Address).IsRequired().HasMaxLength(250);
                b.Property(x => x.Price).HasColumnType("decimal(18,2)");
                b.Property(x => x.CodeInternal).IsRequired().HasMaxLength(30);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.HasOne(x => x.Owner)
                  .WithMany(o => o.Properties)
                  .HasForeignKey(x => x.IdOwner)
                  .HasConstraintName("FK_Property_Owner");
                b.HasIndex(x => x.IdOwner).HasDatabaseName("IX_Property_Owner");
                b.HasIndex(x => x.Price).HasDatabaseName("IX_Property_Price");
            });

            modelBuilder.Entity<PropertyImage>(b =>
            {
                b.ToTable("PropertyImage", "dbo");
                b.HasKey(x => x.IdPropertyImage).HasName("PK_PropertyImage");
                b.Property(x => x.File).IsRequired().HasMaxLength(500);
                b.Property(x => x.Enabled).HasDefaultValue(true);
                b.HasOne(x => x.Property)
                  .WithMany(p => p.Images)
                  .HasForeignKey(x => x.IdProperty)
                  .HasConstraintName("FK_PropertyImage_Property")
                  .OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(x => x.IdProperty).HasDatabaseName("IX_PropertyImage_Property");
            });

            modelBuilder.Entity<PropertyTrace>(b =>
            {
                b.ToTable("PropertyTrace", "dbo");
                b.HasKey(x => x.IdPropertyTrace).HasName("PK_PropertyTrace");
                b.Property(x => x.DateSale).HasConversion(dateOnlyConverter);
                b.Property(x => x.Value).HasColumnType("decimal(18,2)");
                b.Property(x => x.Tax).HasColumnType("decimal(18,2)");
                b.HasOne(x => x.Property)
                  .WithMany(p => p.Traces)
                  .HasForeignKey(x => x.IdProperty)
                  .HasConstraintName("FK_PropertyTrace_Property")
                  .OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(x => new { x.IdProperty, x.DateSale }).HasDatabaseName("IX_PropertyTrace_PropertyDate");
            });
        }
    }
}