using Users.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Users API", Version = "v1" });
});

// Register in-memory storage as singleton
builder.Services.AddSingleton<UserStore>();

var app = builder.Build();

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Map endpoints
app.MapUserEndpoints();

app.Run();

/// <summary>
/// In-memory storage for users.
/// </summary>
public class UserStore
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, User> _users = new();

    public IEnumerable<User> GetAll() => _users.Values.ToList();

    public User? GetById(Guid id) => _users.GetValueOrDefault(id);

    public User Add(User user)
    {
        _users[user.Id] = user;
        return user;
    }

    public User? Update(Guid id, string name, string email)
    {
        if (!_users.TryGetValue(id, out var user))
            return null;

        user.Name = name;
        user.Email = email;
        return user;
    }

    public bool Delete(Guid id) => _users.TryRemove(id, out _);

    public bool Exists(Guid id) => _users.ContainsKey(id);
}

/// <summary>
/// Extension methods for mapping user endpoints.
/// </summary>
public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithOpenApi();

        // GET /api/users - List all users
        group.MapGet("/", (UserStore store) =>
        {
            var users = store.GetAll();
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .WithDescription("Returns all users")
        .Produces<IEnumerable<User>>(StatusCodes.Status200OK);

        // GET /api/users/{id} - Get user by ID
        group.MapGet("/{id:guid}", (Guid id, UserStore store) =>
        {
            var user = store.GetById(id);
            return user is null
                ? Results.NotFound(new { Message = $"User with ID {id} not found" })
                : Results.Ok(user);
        })
        .WithName("GetUserById")
        .WithDescription("Returns a user by ID")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST /api/users - Create a new user
        group.MapPost("/", (CreateUserRequest request, UserStore store) =>
        {
            // Validate input
            var errors = ValidateCreateRequest(request);
            if (errors.Count > 0)
                return Results.BadRequest(new { Errors = errors });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            store.Add(user);
            return Results.Created($"/api/users/{user.Id}", user);
        })
        .WithName("CreateUser")
        .WithDescription("Creates a new user")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // PUT /api/users/{id} - Update an existing user
        group.MapPut("/{id:guid}", (Guid id, UpdateUserRequest request, UserStore store) =>
        {
            // Check if user exists
            if (!store.Exists(id))
                return Results.NotFound(new { Message = $"User with ID {id} not found" });

            // Validate input
            var errors = ValidateUpdateRequest(request);
            if (errors.Count > 0)
                return Results.BadRequest(new { Errors = errors });

            var user = store.Update(id, request.Name.Trim(), request.Email.Trim());
            return Results.Ok(user);
        })
        .WithName("UpdateUser")
        .WithDescription("Updates an existing user")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        // DELETE /api/users/{id} - Delete a user
        group.MapDelete("/{id:guid}", (Guid id, UserStore store) =>
        {
            if (!store.Exists(id))
                return Results.NotFound(new { Message = $"User with ID {id} not found" });

            store.Delete(id);
            return Results.NoContent();
        })
        .WithName("DeleteUser")
        .WithDescription("Deletes a user by ID")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }

    private static List<string> ValidateCreateRequest(CreateUserRequest request) =>
        ValidateNameAndEmail(request.Name, request.Email);

    private static List<string> ValidateUpdateRequest(UpdateUserRequest request) =>
        ValidateNameAndEmail(request.Name, request.Email);

    private static List<string> ValidateNameAndEmail(string name, string email)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required");

        if (string.IsNullOrWhiteSpace(email))
            errors.Add("Email is required");
        else if (!IsValidEmail(email))
            errors.Add("Email format is invalid");

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }
}
