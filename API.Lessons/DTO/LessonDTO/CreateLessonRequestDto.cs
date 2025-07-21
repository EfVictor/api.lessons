using API.Lessons.Models;

namespace API.Lessons.DTO.LessonDTO;

public class CreateLessonRequestDto
{
    public Lesson Lesson { get; set; }
    public string CourseTopicId { get; set; }
    public string CourseCategoryId { get; set; }
}