using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wx.Core.Entities;

namespace wx.Infrastructure.DataConfig;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Ignore(p => p.DomainEvents);

        builder.Property(c => c.Version)
            .IsConcurrencyToken();

        builder.OwnsOne(p => p.BrandModel,brand =>
        {
            brand.Property(b => b.Brand).HasColumnName("Brand");
            brand.Property(b => b.Model).HasColumnName("Model");
        }); ;

        builder.HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Attributes)
           .WithOne(pa => pa.Product)
           .HasForeignKey(pa => pa.ProductId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Images)
         .WithOne(pi => pi.Product)
         .HasForeignKey(pi => pi.ProductId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

