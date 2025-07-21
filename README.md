# api.lessons
REST API example for adding lessons with detailed information

-------

* JSON request for creating a lesson:   

```
{
  "lesson": {
    "title": "Тестовый урок",
    "imageUrl": "https://example.com/lesson.jpg",
    "videoPreviewUrl": "https://example.com/video.jpg",
    "videoUrl": "https://example.com/video.mp4",  
    "text": "Повторение - мать учения",
    "duration": 500,
    "isCompleted": false,
    "courseTopicId": "1"
  },
  "courseTopicId": "1",
  "courseCategoryId": "test"
}
```
  
* JSON request for updating a lesson:  

```
{
  "id": 1,
  "title": "Обновленный тестовый урок",
  "imageUrl": "https://example.com/lesson.jpg",
  "videoPreviewUrl": "https://example.com/video.jpg",
  "videoUrl": "https://example.com/video3.mp4",
  "text": "Обновление - мать учения",
  "duration": 500,
  "isCompleted": false,
  "courseTopicId": "1"
}
```
