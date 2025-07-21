using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Lessons.Models;

public class Lesson
{
    [Key]
    public int? Id { get; set; } 
    public string Title { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoPreviewUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? Text { get; set; }
    public int? Duration { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; } // Soft Delete
    public string? CourseTopicId { get; set; } 
    [JsonIgnore]
    public CourseTopic? CourseTopic { get; set; }
}