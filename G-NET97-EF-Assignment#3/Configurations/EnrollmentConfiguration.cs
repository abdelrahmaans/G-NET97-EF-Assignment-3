using G_NET97_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET97_EF_Assignment_3.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => new { e.StudentId, e.CourseId });
        builder.Property(e => e.EnrolledOn).IsRequired();
        builder.Property(e => e.ProgressPercentage).HasPrecision(5, 2).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.FinalGrade).HasPrecision(5, 2);

        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
