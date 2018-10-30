using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreeColor.Data.Models;

namespace TreeColor.Server.Data.Mapping
{
    public class PointMapping : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Point>
    {
        public void Configure(EntityTypeBuilder<Point> builder)
        {
            builder.ToTable("Points");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");

            builder.Property(s => s.TestId).IsRequired().HasColumnName("testid");
            builder.Property(s => s.Color).IsRequired().HasColumnName("color");
            builder.Property(s => s.Symbol).IsRequired().HasColumnName("symbol");
        }
    }
}