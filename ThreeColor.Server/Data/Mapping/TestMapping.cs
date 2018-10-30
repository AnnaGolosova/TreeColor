using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeColor.Data.Models;

namespace TreeColor.Server.Data.Mapping
{
    public class TestMapping : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Tests");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");

            builder.Property(s => s.Name).IsRequired().HasColumnName("test_name");
            builder.Property(s => s.FieldColor).IsRequired().HasColumnName("field_color");
            builder.Property(s => s.PointSize).IsRequired().HasColumnName("Point_size");
            builder.Property(s => s.Speed).IsRequired().HasColumnName("Speed");
            builder.Property(s => s.MinimumInterval).IsRequired().HasColumnName("int_min");
            builder.Property(s => s.MaximumInterval).IsRequired().HasColumnName("int_max");
        }

    }
}