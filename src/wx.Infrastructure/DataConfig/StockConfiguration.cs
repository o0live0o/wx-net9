using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wx.Core.Entities;

namespace wx.Infrastructure.DataConfig;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .UseHiLo("catalog_type_hilo")
               .IsRequired();

        builder.Property(x => x.Code)
               .IsRequired();

        builder.HasIndex(x => x.Code)
               .IsUnique();
    }
}