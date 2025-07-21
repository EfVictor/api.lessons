using Microsoft.EntityFrameworkCore;
using API.Lessons.Models;

namespace API.Lessons.Data;

public class PostgreDbContext : DbContext
{
    public PostgreDbContext(DbContextOptions options) : base(options) { }


    public DbSet<CourseCategory> CourseCategory { get; set; } = null!;
    public DbSet<CourseTopic> CourseTopic { get; set; } = null!;
    public DbSet<Lesson> Lesson { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.CourseTopic)
            .WithMany(t => t.Lessons)
            .HasForeignKey(l => l.CourseTopicId);

        modelBuilder.Entity<CourseTopic>()
            .HasOne(t => t.CourseCategory)
            .WithMany(c => c.Topics)
            .HasForeignKey(t => t.CourseCategoryId);
    }
}