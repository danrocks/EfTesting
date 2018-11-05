using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EfTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Try some ef stuff");

            SchoolContext context = new SchoolContext();
             context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            var dan = new Student { Name = "Daniel" };
            var maria = new Student { Name = "Maria" };
            var math = new Course { CourseName = "Maths", Students=new List<Student>{ dan, maria} };
            var eng = new Course { CourseName = "English", Students =new  List < Student >{ maria } };


            await context.Courses.AddAsync(math);
            await context.Courses.AddAsync(eng);

            await context.Students.AddAsync( dan);
            await context.Students.AddAsync(maria);



            await context.SaveChangesAsync();
            Console.WriteLine("Courses");
            await context.Courses.ForEachAsync(c => Console.WriteLine("{0}. {1}", c.CourseId, c.CourseName));

            Console.WriteLine("Students");
            await context.Students.ForEachAsync(s => Console.WriteLine("{0}. {1}", s.StudentId, s.Name));

            //context.StudentCourses.Add(new StudentCourse { CourseId = 1,  StudentId=1});
            //context.StudentCourses.Add(new StudentCourse { CourseId = 2, StudentId=2});
            //context.SaveChangesAsync();
            //context.StudentCourses.ForEachAsync(sc => Console.WriteLine("{0}. {1}", sc.StudentId, sc.CourseId));
            
            Console.ReadKey();
            
        }
    }



    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        //public List<Course> Courses { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<Student> Students { get; set; }
    }

    //public class StudentCourse
    //{
    //    public int CourseId { get; set; }
    //    public int StudentId { get; set; }

    //    public Course Course { get; set; }
    //    public Student Student { get; set; }
    //}
    
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
       // public DbSet<StudentCourse> StudentCourses { get; set; }

        public SchoolContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SchoolDB;Trusted_Connection=True;");
            //Model.FindEntityType<StudentCourse>().
            //  
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
        //    modelBuilder.Entity<StudentCourse>()
        //        .HasKey(o => new { o.StudentId, o.CourseId });
        //
        //    modelBuilder.Entity<StudentCourse>().HasOne<Course>().WithOne().HasForeignKey<Course>();
        //    modelBuilder.Entity<StudentCourse>().HasOne<Student>().WithOne().HasForeignKey<Student>();


        }


    }
}
