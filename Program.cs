using Microsoft.EntityFrameworkCore;
using TodoApp;
using TodoApp.routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Db>(options =>
    options.UseNpgsql("Host=localhost;Port=6432;Username=example;Password=example;Database=example"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Db>();
    var canConnect = await db.Database.CanConnectAsync();
    app.Logger.LogInformation("Can connect to database: {CanConnect}", canConnect);

    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
}

app.UseStaticFiles();
app.MapFallbackToFile("index.html");


// ----------- ПРАВИЛЬНЫЕ МАРШРУТЫ -----------
var api = app.MapGroup("/api/todos");

app.MapGet("/", TodoRoutes.GetAllTodoss); // Рендер Razor-страницы

// JSON API
api.MapGet("/", TodoRoutes.GetAllTodos);
api.MapGet("/complete", TodoRoutes.GetCompleteTodos);
api.MapGet("/{id:int}", TodoRoutes.GetTodo);   // <-- id должен быть int
app.MapPost("/api", (Todo newTodo) =>
{
    // например сохраняешь в БД или списке
    return Results.Ok(newTodo);
});
api.MapPut("/{id:int}", TodoRoutes.UpdateTodo);
api.MapDelete("/{id:int}", TodoRoutes.DeleteTodo);
api.MapDelete("/", TodoRoutes.DeleteAllTodos);
// -------------------------------------------

app.Run();