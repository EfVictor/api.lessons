using API.Lessons.DTO.LessonDTO;

namespace API.Lessons.Models;

public class FullLessonTask  
{
    public List<CourseCategory> Categories { get; set; } = new List<CourseCategory>();
    public PaginatedCourseResponseDto Pagination { get; set; }  
}