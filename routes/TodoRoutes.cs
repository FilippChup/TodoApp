using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Razor.Templating.Core;
using TodoApp.Views;

namespace TodoApp.routes;

public class TodoRoutes
{
    public static async Task<IResult> GetAllTodos([FromServices] Db db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
    }
    
    public static async Task<IResult> GetAllTodoss(Db db)
    {
        var todos = await db.Todos.Select(x => new TodoItemDTO(x)).ToListAsync();
        

        var html = await RazorTemplateEngine.RenderAsync("Views/Todos.cshtml", todos);
        
        
        
        return Results.Content(html, "text/html");
    }

    public static async Task<IResult> GetCompleteTodos([FromServices]Db db) {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
    }

    public static async Task<IResult> GetTodo(int id, [FromServices] Db db)
    {
        return await db.Todos.FindAsync(id)
            is Todo todo
            ? TypedResults.Ok(new TodoItemDTO(todo))
            : TypedResults.NotFound();
    }

    public static async Task<IResult> CreateTodo(
        [FromBody] TodoItemDTO todoItemDTO,
        [FromServices] Db db,
        [FromServices] ILogger<TodoRoutes> logger
        )
    {
        logger.LogInformation("Log from CreateTodo, {1}, {2}", todoItemDTO.Name, todoItemDTO.Priority);
        var todoItem = new Todo
        {
            IsComplete = todoItemDTO.IsComplete,
            Name = todoItemDTO.Name,
            Secret = todoItemDTO.Secret,
            Priority = todoItemDTO.Priority,
        };

        db.Todos.Add(todoItem);
        await db.SaveChangesAsync();

        todoItemDTO = new TodoItemDTO(todoItem);

        return TypedResults.Created($"/api/todoitems/{todoItem.Id}", todoItemDTO);
    }
    public static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, [FromServices] Db db)
{
    // Найдем задачу по ID
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();  // Если задача не найдена

    // Проверка, что переданы корректные данные
    if (string.IsNullOrEmpty(todoItemDTO.Name))
    {
        return TypedResults.BadRequest("Task name cannot be empty.");
    }

    // Обновление полей задачи
    todo.Name = todoItemDTO.Name;
    todo.Priority = todoItemDTO.Priority ?? todo.Priority;  // Если приоритет не передан, оставляем старое значение
    todo.IsComplete = todoItemDTO.IsComplete;

    await db.SaveChangesAsync();  // Сохраняем изменения в БД

    // Возвращаем обновленную задачу
    var updatedTodoDTO = new TodoItemDTO(todo);
    return TypedResults.Ok(updatedTodoDTO);
}

    public static async Task<IResult> DeleteTodo(
        int id, 
        [FromServices] Db db,
        [FromServices] ILogger<TodoRoutes> logger
        )
    
    {
        logger.LogInformation("Log from DeleteTodo");
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    
        return TypedResults.NotFound();
    }

    public static async Task<IResult> DeleteAllTodos([FromServices] Db db)
    {
        var all = db.Todos.ToList();
        db.Todos.RemoveRange(all);
        await db.SaveChangesAsync();
    
        return TypedResults.NoContent();
    }
    
    // public class TodoApiController : ControllerBase
    // {
    //     private readonly AppDbContext _db;
    //     public TodoApiController(AppDbContext db)
    //     {
    //         _db = db;
    //     }
    //
    //     [HttpGet("/api/")]
    //     public async Task<IActionResult> GetAll()
    //     {
    //         var todos = await _db.Todos
    //             .Select(t => new TodoItemDTO
    //             {
    //                 Id = t.Id,
    //                 Name = t.Name,
    //                 Priority = t.Priority,
    //                 IsComplete = t.IsComplete
    //             }).ToListAsync();
    //
    //         return Ok(todos);
    //     }
    //
    //     [HttpPost("/api/")]
    //     public async Task<IActionResult> Create([FromBody] TodoItemDTO todo)
    //     {
    //         if (todo == null || string.IsNullOrEmpty(todo.Name))
    //             return BadRequest();
    //     
    //         var newTodo = new TodoItem
    //         {
    //             Name = todo.Name,
    //             Priority = todo.Priority,
    //             IsComplete = false
    //         };
    //     
    //         _db.Todos.Add(newTodo);
    //         await _db.SaveChangesAsync();
    //     
    //         return Ok(new TodoItemDTO
    //         {
    //             Id = newTodo.Id,
    //             Name = newTodo.Name,
    //             Priority = newTodo.Priority,
    //             IsComplete = newTodo.IsComplete
    //         });
    //     }
    //     
    //     [HttpPut("/api/{id}/toggle")]
    //     public async Task<IActionResult> ToggleComplete(int id)
    //     {
    //         var todo = await _db.Todos.FindAsync(id);
    //         if (todo == null)
    //             return NotFound();
    //
    //         todo.IsComplete = !todo.IsComplete;
    //         await _db.SaveChangesAsync();
    //
    //         return Ok(new TodoItemDTO
    //         {
    //             Id = todo.Id,
    //             Name = todo.Name,
    //             Priority = todo.Priority,
    //             IsComplete = todo.IsComplete
    //         });
    //     }
    // }
}