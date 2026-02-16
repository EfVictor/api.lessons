using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Lessons.DTO.LessonDTO;
using API.Lessons.Interfaces;
using API.Lessons.Models;
using API.Lessons.Data;

public class FullLessonTaskService : IFullLessonsTaskService
{
    private readonly PostgreDbContext _context;
    private readonly ILogger<FullLessonTaskService> _logger;
    private readonly IMapper _mapper;

    public FullLessonTaskService(PostgreDbContext db, ILogger<FullLessonTaskService> logger, IMapper mapper)
    {
        _context = db;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<FullLessonTask> GetAllAsync( 
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            if (pageSize < 1) pageSize = 10;
            if (page < 1) page = 1;
            var totalCount = await _context.CourseCategory.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var categories = await _context.CourseCategory
                .Include(c => c.Topics)
                    .ThenInclude(t => t.Lessons)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new FullLessonTask
            {
                Categories = categories,
                Pagination = new PaginatedCourseResponseDto
                { 
                    CurrentPage = page,
                    TotalPages = totalPages
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAllLesson: Error get lessons. Error: {{Error}}: {ex.Message}");
            throw;
        }
    }
    public async Task<Lesson?> GetLessonAsync(int lessonId)
    {
        try
        {
            _logger.LogInformation($"Fetching lesson with ID: {lessonId}");
            var item = await _context.Lesson
                .Include(l => l.CourseTopic) 
                .ThenInclude(t => t.CourseCategory) 
                .FirstOrDefaultAsync(l => l.Id == lessonId);
            if (item == null)
            {
                _logger.LogWarning("Lesson not found with ID: {LessonId}", lessonId);
                return null;
            }
            var result = new Lesson
            {
                Id = item.Id,
                Title = item.Title,
                ImageUrl = item.ImageUrl,
                VideoPreviewUrl = item.VideoPreviewUrl,
                VideoUrl = item.VideoUrl,
                Text = item.Text,
                Duration = item.Duration,
                IsCompleted = item.IsCompleted,
            };
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to fetch lesson with ID: {lessonId}");
            throw;
        }
    }
    public async Task CreateLessonAsync(Lesson lesson, string topicId, string categoryId) 
    {
        try
        {
            _logger.LogInformation("Creating a new lesson...");
            var existingCategory = await _context.CourseCategory.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (existingCategory == null)
            {
                _logger.LogInformation($"Сategory not found with ID '{categoryId}'");
            }
            var existingTopic = await _context.CourseTopic.FirstOrDefaultAsync(t => t.Id == topicId);
            if (existingTopic == null)
            {
                _logger.LogInformation($"Topic not found with ID '{topicId}'");
            }
            lesson.CourseTopicId = topicId;
            await _context.Lesson.AddAsync(lesson);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Lesson created successfully: {lesson.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create lesson");
            throw;
        }
    }
    public async Task UpdateLessonAsync(int lessonId, Lesson updatedLesson)
    {
        try
        {
            _logger.LogInformation($"Updating lesson with ID: {lessonId}");
            var existingOrder = await _context.Lesson.FirstOrDefaultAsync(o => o.Id == lessonId);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Lesson with ID {lessonId} not found.");
            }
            _context.Entry(existingOrder).CurrentValues.SetValues(updatedLesson);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Lesson updated successfully: {lessonId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to update lesson with ID: {lessonId}");
            throw;
        }
    }
    public async Task DeleteLessonAsync(int lessonId)
    {
        try
        {
            _logger.LogInformation($"Deleting lesson with ID: {lessonId}");
            var lesson = await _context.Lesson.FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null)
            {
                throw new KeyNotFoundException($"Lesson with ID {lessonId} not found.");
            }
            // Soft Delete
            //lesson.IsDeleted = true;  
            //_context.Lesson.Update(lesson)
            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Lesson deleted successfully: {lessonId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete lesson with ID: {lessonId}");
            throw;
        }
    }
}