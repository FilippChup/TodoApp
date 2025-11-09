using Microsoft.EntityFrameworkCore;
using TodoApp;
using TodoApp.routes;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Db>((sp, options) =>
{
    options.UseNpgsql("Host=localhost;Port=6432;Username=example;Password=example;Database=example");
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost;Port=6432;Username=example;Password=example;Database=example")));

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<Db>();
var canConnect = await db.Database.CanConnectAsync();
app.Logger.LogInformation("Can connect to database: {CanConnect}", canConnect);

await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

RouteGroupBuilder todoItems = app.MapGroup("");

app.MapGet("/", () => Results.Redirect("/index.html"));
todoItems.MapGet("/api/allTasks", TodoRoutes.GetAllTodos);
todoItems.MapGet("/api/tesst", TodoRoutes.GetAllTodoss);
todoItems.MapGet("/api/complete",TodoRoutes.GetCompleteTodos);
todoItems.MapGet("/api/{id}", TodoRoutes.GetTodo);
todoItems.MapPost("/api/", TodoRoutes.CreateTodo);
todoItems.MapPut("/api/{id}", TodoRoutes.UpdateTodo);
todoItems.MapDelete("/api/{id}",TodoRoutes.DeleteTodo);
todoItems.MapDelete("/api/", TodoRoutes.DeleteAllTodos);


app.Run();
