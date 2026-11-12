using G_NET97_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET97_EF_Assignment_3.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.FullName).IsRequired().HasMaxLength(150);
        builder.Property(i => i.Email).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Bio).HasMaxLength(500);
        builder.Property(i => i.JoinedAt).IsRequired();

        builder.HasData(
            new Instructor { Id = 1, FullName = "Ahmed Hassan", Email = "ahmed@example.com", Bio = "Backend instructor", JoinedAt = new DateTime(2024, 1, 10) },
            new Instructor { Id = 2, FullName = "Mona Ali", Email = "mona@example.com", Bio = "Frontend instructor", JoinedAt = new DateTime(2024, 2, 15) },
            new Instructor { Id = 3, FullName = "Omar Samir", Email = "omar@example.com", Bio = null, JoinedAt = new DateTime(2024, 3, 20) }
        );
    }
}
