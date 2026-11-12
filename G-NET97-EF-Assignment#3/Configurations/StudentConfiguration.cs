using G_NET97_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET97_EF_Assignment_3.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.FullName).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Email).IsRequired().HasMaxLength(200);
        builder.Property(s => s.DateOfBirth).IsRequired();
    }
}
