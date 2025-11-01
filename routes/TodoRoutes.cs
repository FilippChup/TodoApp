using Microsoft.EntityFrameworkCore;

namespace TodoApp.routes;

public class TodoRoutes
{
    public static async Task<IResult> GetAllTodos(Db db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
    }

    public static async Task<IResult> GetCompleteTodos(Db db) {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
    }

    public static async Task<IResult> GetTodo(int id, Db db)
    {
        return await db.Todos.FindAsync(id)
            is Todo todo
            ? TypedResults.Ok(new TodoItemDTO(todo))
            : TypedResults.NotFound();
    }

    public static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, Db db)
    {
        var todoItem = new Todo
        {
            IsComplete = todoItemDTO.IsComplete,
            Name = todoItemDTO.Name,
            Secret = todoItemDTO.Secret
        };

        db.Todos.Add(todoItem);
        await db.SaveChangesAsync();

        todoItemDTO = new TodoItemDTO(todoItem);

        return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
    }

    public static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, Db db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = todoItemDTO.Name;
        todo.IsComplete = todoItemDTO.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteTodo(int id, Db db)
    {
        if (await db.Todos.FindAsync(id) is Todo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    public static async Task<IResult> DeleteAllTodos(Db db)
    {
        var all = db.Todos.ToList();
        db.Todos.RemoveRange(all);
        await db.SaveChangesAsync();
    
        return TypedResults.NoContent();
    }


}