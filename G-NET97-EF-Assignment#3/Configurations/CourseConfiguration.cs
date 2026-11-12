using G_NET97_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET97_EF_Assignment_3.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Price).HasPrecision(10, 2);
        builder.Property(c => c.Level).IsRequired();
        builder.Property(c => c.IsPublished).IsRequired();

        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Category)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Course { Id = 1, Title = "C# Fundamentals", Description = "Learn C# basics", Price = 500m, Level = CourseLevel.Beginner, IsPublished = true, PublishedAt = new DateTime(2025, 1, 5), InstructorId = 1, CategoryId = 1 },
            new Course { Id = 2, Title = "ASP.NET Core Web API", Description = "Build Web APIs", Price = 1200m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedAt = new DateTime(2025, 2, 10), InstructorId = 1, CategoryId = 5 },
            new Course { Id = 3, Title = "Advanced EF Core", Description = "Advanced data access", Price = 1500m, Level = CourseLevel.Advanced, IsPublished = false, PublishedAt = null, InstructorId = 3, CategoryId = 3 },
            new Course { Id = 4, Title = "HTML and CSS", Description = "Web design foundations", Price = 400m, Level = CourseLevel.Beginner, IsPublished = true, PublishedAt = new DateTime(2025, 3, 1), InstructorId = 2, CategoryId = 4 },
            new Course { Id = 5, Title = "JavaScript Essentials", Description = "Modern JavaScript", Price = 700m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedAt = new DateTime(2025, 3, 15), InstructorId = 2, CategoryId = 4 },
            new Course { Id = 6, Title = "UI Design Masterclass", Description = "Design user interfaces", Price = 900m, Level = CourseLevel.Advanced, IsPublished = false, PublishedAt = null, InstructorId = 3, CategoryId = 2 }
        );
    }
}
