using System.ComponentModel.DataAnnotations;

namespace API.Lessons.Models;

public class CourseCategory  
{
    [Key]
    public string Id { get; set; }
    public string Title { get; set; }
    public List<CourseTopic> Topics { get; set; } = new List<CourseTopic>();
}