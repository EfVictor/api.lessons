using Microsoft.AspNetCore.Mvc;
using API.Lessons.Interfaces;
using API.Lessons.Models;
using API.Lessons.DTO.LessonDTO;

namespace API.Lessons.Controllers
{
    [Route("api/v1/Lessons")]
    [ApiController]

    public class LessonForAdminController : ControllerBase
    {
        private readonly IFullLessonsTaskService _service;
        private readonly ILogger<LessonForAdminController> _logger;

        public LessonForAdminController(IFullLessonsTaskService service, ILogger<LessonForAdminController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("all")]
        public async Task<ActionResult<FullLessonTask>> GetAll(
                    [FromQuery] int page = 1,
                    [FromQuery] int pagesize = 10)
        {
            try
            {
                var result = await _service.GetAllAsync(page, pagesize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAllLesson: Error get lessons. Error: {Error}",ex.Message);
                return StatusCode(500, "Internal server error.");
            } 
        }
        [HttpGet("{lessonId:int}")]
        public async Task<ActionResult> GetLesson(int lessonId)
        {
            _logger.LogInformation("GetLesson: Fetching lesson with LessonId {LessonId}", lessonId);
            var order = await _service.GetLessonAsync(lessonId);
            if (order == null)
            {
                _logger.LogWarning("GetLesson: Lesson with LessonId {LessonId} not found.", lessonId);
                return NotFound(new { Error = "Lesson not found." });
            }
            _logger.LogInformation("GetLesson: Successfully retrieved lesson with LessonId {LessonId}", lessonId);
            return Ok(order);
        }
            
        [HttpPost("create")] 
        public async Task<ActionResult<int>> CreateLesson([FromBody] CreateLessonRequestDto request) 
        {
            
            if (!ModelState.IsValid)  
            {
                foreach (var error in ModelState)
                {
                    _logger.LogError($"Validation error: {error.Key} -> {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return BadRequest(ModelState);
            }
            _logger.LogInformation("CreateLesson: Creating a new lesson.");

            request.Lesson.CourseTopicId = request.CourseTopicId;
            await _service.CreateLessonAsync(request.Lesson, request.CourseTopicId, request.CourseCategoryId);

            _logger.LogInformation("CreateLesson: Successfully created lesson with lessonId {LessonId}", request.Lesson.Id);

            return CreatedAtAction(nameof(GetLesson), new { lessonId = request.Lesson.Id },
                new { Message = "Lesson created successfully.", LessonId = request.Lesson.Id });
        }

        [HttpPut("update/{lessonId:int}")] 
        public async Task<ActionResult> UpdateLesson(int lessonId, [FromBody] Lesson lessonDto)
        {
            if (lessonDto == null)
            {
                return BadRequest(new { Error = "Lesson data is null." });
            }
            try
            {
                await _service.UpdateLessonAsync(lessonId, lessonDto);
                return Ok(new { Message = "Lesson updated successfully.", LessonId = lessonId });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("UpdateLesson: Lesson with LessonId {LessonId} not found. Error: {Error}", lessonId,ex.Message);
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpDelete("delete/{lessonId:int}")]
        public async Task<ActionResult> DeleteLesson(int lessonId)
        {
            try
            {
                await _service.DeleteLessonAsync(lessonId);
                return Ok(new { Message = "Lesson deleted successfully.", LessonId = lessonId });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteLesson: Error delete lessons. Error: {Error}", ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        } 
        [HttpGet("forUser")]
        public async Task<ActionResult<FullLessonTask>> GetAllforUser(
                    [FromQuery] int page = 1,
                    [FromQuery] int pagesize = 10)
        {
            try
            {
                var result = await _service.GetAllAsync(page, pagesize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetForUser: Error get lessons. Error: {Error}", ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}