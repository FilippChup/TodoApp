using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers;

[ApiController]
[Route("/todoitems")]
public class TodosController : ControllerBase
{
    private static List<Todo> _todos = 
    [
        new Todo
        {
            Id = 1,
            Title = "my new todo"
        }
    ];
    
    [HttpGet]
    public IActionResult GetTodoById([FromQuery] int todoId)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == todoId);
        return Ok(todo);
    }
}

class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
}