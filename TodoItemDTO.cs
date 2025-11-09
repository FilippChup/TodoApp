using Microsoft.EntityFrameworkCore;

public class TodoItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Priority { get; set; }
    public bool IsComplete { get; set; }
}

public class TodoItemDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
    public string Priority { get; set; }
    public TodoItemDTO() { }
    public TodoItemDTO(Todo todoItem) =>
        (Id, Name, IsComplete, Secret, Priority) = (todoItem.Id, todoItem.Name, todoItem.IsComplete, todoItem.Secret, todoItem.Priority);
}