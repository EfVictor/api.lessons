using API.Lessons.Models;

namespace API.Lessons.Interfaces;

public interface IFullLessonsTaskService
{
    Task<FullLessonTask> GetAllAsync( int page = 1,int pageSize = 10);
    Task<Lesson?> GetLessonAsync(int lessonId);
    Task CreateLessonAsync(Lesson lesson, string topicId, string categoryId);
    Task UpdateLessonAsync(int lessonId, Lesson updatedLesson);
    Task DeleteLessonAsync(int lessonId);
}