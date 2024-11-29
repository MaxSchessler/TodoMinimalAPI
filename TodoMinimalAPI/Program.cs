using Microsoft.EntityFrameworkCore;
using TodoMinimalAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


app.MapGet("/todoitems", async(TodoDB db) => 
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDB db) =>
    await db.Todos.Where(t => t.isComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDB db) => 
    await db.Todos.FindAsync(id)
        is Todo todo 
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDB db) =>
{
    if (todo != null 
    && todo.Name != null)
    {
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return Results.Created($"/todoitems{todo.Id}", todo);
    } 
    else
    {
        return Results.BadRequest(new
        {
            ErrorMessage = "Todo does not contain a name"
        });
    }
});

app.MapPut("/todoitems/complete/{id}", async (int id, TodoDB db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        todo.isComplete = true;
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    } else
    {
        return Results.NotFound($"No todo with ID: {id}.");
    }
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDB db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.BadRequest(new
    {
        ErrorMessage = $"Todo with ID: {id} does not exist."
    });
});

app.Run();
