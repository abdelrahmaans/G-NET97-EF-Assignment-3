namespace G_NET97_EF_Assignment_3.Models;

public class Enrollment
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime EnrolledOn { get; set; }
    public decimal ProgressPercentage { get; set; }
    public EnrollmentStatus Status { get; set; }
    public decimal? FinalGrade { get; set; }
}
