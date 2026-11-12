using G_NET97_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using AppContext = G_NET97_EF_Assignment_3.Data.AppContext;

namespace G_NET97_EF_Assignment_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using AppContext context = new AppContext();

            #region Create

            AddInstructor(context);

            AddCourse(context);

            AddSeveralStudents(context);

            EnrollStudent(context);

            #endregion

            #region Read

            FindCourse(context);

            FilterCourses(context, 400m, 1200m);

            PrintSubCategories(context, 1);

            PrintTopLevelCategories(context);

            PrintCourseRoster(context, 1);

            #endregion

            #region Update

            ChangeCoursePrice(context, 1, 550m);

            UpdateEnrollmentProgress(context, 1, 1, 100m);

            PublishCourse(context, 1);

            #endregion

            #region Delete

            UnenrollStudent(context, 1, 1);

            TryDeleteCategory(context, 1);

            SafeDeleteCategory(context, 1);

            DeleteCourse(context, 1);

            #endregion



        }
        #region Methods
        #region Create

        // 1. Add an instructor
        static void AddInstructor(AppContext context)
        {
            Instructor instructor = new Instructor()
            {
                FullName = "Ahmed Hassan",
                Email = "ahmed@example.com",
                Bio = "Backend Developer",
                JoinedAt = DateTime.Now
            };

            context.Instructors.Add(instructor);

            context.SaveChanges();

            Console.WriteLine(
                $"Instructor added successfully. Id: {instructor.Id}");
        }

        // 2. Add a course
        static void AddCourse(AppContext context)
        {
            int instructorId = 1;
            int categoryId = 1;

            bool instructorExists = context.Instructors
                .Any(i => i.Id == instructorId);

            bool categoryExists = context.Categories
                .Any(c => c.Id == categoryId);

            if (!instructorExists)
            {
                Console.WriteLine("Course was not added: Instructor not found.");
                return;
            }

            if (!categoryExists)
            {
                Console.WriteLine("Course was not added: Category not found.");
                return;
            }

            Course course = new Course()
            {
                Title = "C# Fundamentals",
                Description = "Learn C# from basics",
                Price = 500m,
                Level = CourseLevel.Beginner,
                IsPublished = false,
                PublishedAt = null,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            context.Courses.Add(course);

            context.SaveChanges();

            Console.WriteLine(
                $"Course added successfully. Id: {course.Id}");
        }

        // 3. Add several students
        static void AddSeveralStudents(AppContext context)
        {
            List<Student> students = new List<Student>()
            {
                new Student()
                {
                    FullName = "Ali Mahmoud",
                    Email = "ali@example.com",
                    DateOfBirth = new DateTime(2001, 4, 12)
                },

                new Student()
                {
                    FullName = "Sara Ahmed",
                    Email = "sara@example.com",
                    DateOfBirth = new DateTime(2002, 7, 8)
                },

                new Student()
                {
                    FullName = "Youssef Adel",
                    Email = "youssef@example.com",
                    DateOfBirth = new DateTime(2000, 9, 20)
                }
            };

            context.Students.AddRange(students);

            context.SaveChanges();

            foreach (Student student in students)
            {
                Console.WriteLine(
                    $"Student added successfully. Id: {student.Id}");
            }
        }

        // 4. Enroll a student
        static void EnrollStudent(AppContext context)
        {
            int studentId = 1;
            int courseId = 1;

            bool studentExists = context.Students
                .Any(s => s.Id == studentId);

            if (!studentExists)
            {
                Console.WriteLine("Enrollment refused: Student not found.");
                return;
            }

            Course? course = context.Courses
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                Console.WriteLine("Enrollment refused: Course not found.");
                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine("Enrollment refused: Course is not published.");
                return;
            }

            bool alreadyEnrolled = context.Enrollments
                .Any(e => e.StudentId == studentId &&
                          e.CourseId == courseId);

            if (alreadyEnrolled)
            {
                Console.WriteLine("Enrollment refused: Student is already enrolled.");
                return;
            }

            Enrollment enrollment = new Enrollment()
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrolledOn = DateTime.Now,
                ProgressPercentage = 0,
                Status = EnrollmentStatus.Active
            };

            context.Enrollments.Add(enrollment);

            context.SaveChanges();

            Console.WriteLine("Student enrolled successfully.");
        }

        #endregion

        #region Read

        // 1. Find a course
        static void FindCourse(AppContext context)
        {
            int courseId = 1;

            Course? courseById = context.Courses.Find(courseId);

            if (courseById == null)
            {
                Console.WriteLine("Course not found by Id.");
            }
            else
            {
                Console.WriteLine(
                    $"Course found by Id: {courseById.Title}");
            }

            string courseTitle = "C# Fundamentals";

            Course? courseByTitle = context.Courses
                .FirstOrDefault(c => c.Title == courseTitle);

            if (courseByTitle == null)
            {
                Console.WriteLine("Course not found by Title.");
            }
            else
            {
                Console.WriteLine(
                    $"Course found by Title: {courseByTitle.Title}");
            }
        }

        // 2. Filter courses
        static void FilterCourses(
            AppContext context,
            decimal minPrice,
            decimal maxPrice)
        {
            var courses = context.Courses
                .Where(c => c.Price >= minPrice &&
                            c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new
                {
                    c.Title,
                    c.Level,
                    c.Price
                })
                .ToList();

            if (courses.Count == 0)
            {
                Console.WriteLine("No courses found in this price range.");
                return;
            }

            foreach (var course in courses)
            {
                Console.WriteLine(
                    $"Title: {course.Title} | Level: {course.Level} | Price: {course.Price}");
            }
        }

        // 3. Print direct sub-categories
        static void PrintSubCategories(
            AppContext context,
            int categoryId)
        {
            var subCategories = context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .ToList();

            Console.WriteLine(
                $"Sub-categories of Category {categoryId}:");

            if (subCategories.Count == 0)
            {
                Console.WriteLine("No sub-categories found.");
                return;
            }

            foreach (var category in subCategories)
            {
                Console.WriteLine(category.Name);
            }
        }

        // 4. Print top-level categories
        static void PrintTopLevelCategories(
            AppContext context)
        {
            var categories = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            Console.WriteLine("Top-level Categories:");

            if (categories.Count == 0)
            {
                Console.WriteLine("No top-level categories found.");
                return;
            }

            foreach (var category in categories)
            {
                Console.WriteLine(category.Name);
            }
        }

        // 5. Print course roster without joins
        static void PrintCourseRoster(
            AppContext context,
            int courseId)
        {
            bool courseExists = context.Courses
                .Any(c => c.Id == courseId);

            if (!courseExists)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            // First, get student IDs from enrollments
            List<int> studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            Console.WriteLine(
                $"Number of enrolled students: {studentIds.Count}");

            if (studentIds.Count == 0)
            {
                Console.WriteLine("No students enrolled in this course.");
                return;
            }

            // Then, get students using Contains
            var students = context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToList();

            foreach (var student in students)
            {
                Console.WriteLine(student.FullName);
            }
        }

        #endregion

        #region Update

        // 1. Change course price
        static void ChangeCoursePrice(
            AppContext context,
            int courseId,
            decimal newPrice)
        {
            Course? course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Cannot update price: Course not found.");
                return;
            }

            if (newPrice < 0)
            {
                Console.WriteLine("Invalid price. Price cannot be negative.");
                return;
            }

            course.Price = newPrice;

            context.SaveChanges();

            Console.WriteLine("Course price updated successfully.");
        }

        // 2. Update enrollment progress
        static void UpdateEnrollmentProgress(
            AppContext context,
            int studentId,
            int courseId,
            decimal newProgress)
        {
            if (newProgress < 0 || newProgress > 100)
            {
                Console.WriteLine(
                    "Invalid progress. Progress must be between 0 and 100.");
                return;
            }

            // Find enrollment using its composite key
            Enrollment? enrollment = context.Enrollments
                .Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            enrollment.ProgressPercentage = newProgress;

            if (newProgress == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }

            context.SaveChanges();

            Console.WriteLine("Enrollment progress updated successfully.");
        }

        // 3. Publish a course
        static void PublishCourse(
            AppContext context,
            int courseId)
        {
            Course? course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Cannot publish: Course not found.");
                return;
            }

            course.IsPublished = true;
            course.PublishedAt = DateTime.Now;

            context.SaveChanges();

            Console.WriteLine("Course published successfully.");
        }

        #endregion

        #region Delete

        // 1. Unenroll a student
        static void UnenrollStudent(
            AppContext context,
            int studentId,
            int courseId)
        {
            Enrollment? enrollment = context.Enrollments
                .Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            context.Enrollments.Remove(enrollment);

            context.SaveChanges();

            Console.WriteLine("Student unenrolled successfully.");
        }

        // 2. Try deleting a category that has sub-categories
        static void TryDeleteCategory(
            AppContext context,
            int categoryId)
        {
            Category? category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            context.Categories.Remove(category);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Category deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Restore the entity state after the failed delete
                context.Entry(category).State = EntityState.Unchanged;

                Console.WriteLine(
                    $"Category could not be deleted: {ex.Message}");
            }
        }

        // 3. Safe category deletion
        static void SafeDeleteCategory(
            AppContext context,
            int categoryId)
        {
            Category? category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            bool hasSubCategories = context.Categories
                .Any(c => c.ParentCategoryId == categoryId);

            if (hasSubCategories)
            {
                Console.WriteLine(
                    "Category cannot be deleted because it has sub-categories.");
                return;
            }

            context.Categories.Remove(category);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Category deleted successfully.");
            }
            catch (DbUpdateException)
            {
                context.Entry(category).State = EntityState.Unchanged;

                Console.WriteLine(
                    "Category cannot be deleted because it has related courses.");
            }
        }

        // 4. Delete a course and check its enrollments
        static void DeleteCourse(
            AppContext context,
            int courseId)
        {
            Course? course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            int enrollmentsBefore = context.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine(
                $"Enrollments before deletion: {enrollmentsBefore}");

            context.Courses.Remove(course);

            try
            {
                context.SaveChanges();

                int enrollmentsAfter = context.Enrollments
                    .Count(e => e.CourseId == courseId);

                Console.WriteLine(
                    $"Enrollments after deletion: {enrollmentsAfter}");

                // Enrollments were removed because the configured
                // foreign key relationship uses cascade delete.
            }
            catch (DbUpdateException)
            {
                context.Entry(course).State = EntityState.Unchanged;

                Console.WriteLine(
                    "Course could not be deleted because related records prevent deletion.");
            }
        }

        #endregion
        #endregion

    }
}
