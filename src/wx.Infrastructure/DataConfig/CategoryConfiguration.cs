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

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Childrens)
            .HasPrincipalKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.ParentId);
    }
}
