using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TreeColor.Server.Data.Mapping
{
    //public class ResultMapping : IEntityTypeConfiguration<Result>
    //{
    //    public void Configure(EntityTypeBuilder<Result> builder)
    //    {
    //        builder.ToTable("Results");

    //        builder.HasKey(s => s.Id);
    //        builder.Property(s => s.Id).HasColumnName("id");

    //        builder.Property(s => s.TestingNumber).IsRequired().HasColumnName("testingnumber");
    //        builder.Property(s => s.UserId).IsRequired().HasColumnName("userid");
    //        builder.Property(s => s.PointId).IsRequired().HasColumnName("pointid");
    //        builder.Property(s => s.Time).IsRequired().HasColumnName("tim");
    //        builder.Property(s => s.ErrorCode).IsRequired().HasColumnName("error");
    //        builder.Property(s => s.Date).IsRequired().HasColumnName("CDate");

    //        builder.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId);
    //        builder.HasOne(s => s.Point).WithMany().HasForeignKey(s => s.PointId);
    //    }
    //}
}