using API.Lessons.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IFullLessonsTaskService, FullLessonTaskService>();  // Registering application services
builder.Services.AddControllers(); // Adding Controllers to an Application

// Setting up logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
app.MapControllers(); // Setting up controller routing
app.Run();