using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Lessons.Models;

public class CourseTopic
 {
    [Key]
    public string Id { get; set; }    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsCompleted { get; set; }
    public List<Lesson> Lessons { get; set; } = new List<Lesson>();
    public string CourseCategoryId { get; set; }  
    [JsonIgnore]
    public CourseCategory CourseCategory { get; set; }
}