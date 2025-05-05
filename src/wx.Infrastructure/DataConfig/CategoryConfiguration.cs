using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wx.Core.Entities;

namespace wx.Infrastructure.DataConfig;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.Ignore(p => p.DomainEvents);

        builder.Property(c => c.Version)
            .IsConcurrencyToken();

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Childrens)
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Attributes)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.ParentId);
    }
}

