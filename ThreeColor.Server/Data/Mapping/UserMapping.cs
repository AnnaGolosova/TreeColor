using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TreeColor.Data.Models;

namespace TreeColor.Server.Data.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id");

            builder.Property(s => s.Age).IsRequired().HasColumnName("Age");
            builder.Property(s => s.Activity).IsRequired().HasColumnName("Activity");
            builder.Property(s => s.Gender).IsRequired().HasColumnName("Gender");
            builder.Property(s => s.NewId).IsRequired().HasColumnName("NewId");
        }

    }
}