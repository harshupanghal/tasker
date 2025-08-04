// Tasker.Ui/Program.cs
using Microsoft.AspNetCore.Components.Authorization; // Already there, good.
using Tasker.Application.Interfaces;
using Tasker.Application.UseCases.Tasks;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories;
using Tasker.Infrastructure.Services;
using Tasker.Ui.Components; // Ensure this is correct for your App.razor location
using Tasker.UI.Services; // Add this for your CustomAuthenticationStateProvider
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // This is correct for Blazor Server interactivity

// --- START: Authentication Services ---
// Configure Cookie Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies"; // Define the default authentication scheme
})
    .AddCookie("Cookies", options => // Add a cookie authentication handler named "Cookies"
    {
        options.Cookie.Name = "TaskerAuthCookie"; // Name of the authentication cookie
        options.LoginPath = "/login"; // Path to redirect unauthenticated users
        options.AccessDeniedPath = "/AccessDenied"; // Optional: Path for users who are authenticated but not authorized
        options.SlidingExpiration = true; // Renew cookie timeout if user is active
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expiration time
    });

builder.Services.AddAuthorization(); // Adds authorization services

// Register your custom AuthenticationStateProvider and its dependencies
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState(); // Makes AuthenticationState available to components via [CascadingParameter]

// --- END: Authentication Services ---


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    {
    throw new InvalidOperationException("DefaultConnection connection string is not configured.");
    }
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

// Your existing Application/Infrastructure DI registrations
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IAuthService, AuthService>(); // Your AuthService
builder.Services.AddScoped<CreateTaskUseCase>();
builder.Services.AddScoped<GetTasksForUserUseCase>();
builder.Services.AddScoped<GetTaskByIdUseCase>();
builder.Services.AddScoped<UpdateTaskUseCase>();
builder.Services.AddScoped<DeleteTaskUseCase>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();

// --- IMPORTANT: Authentication and Authorization Middleware Order ---
// These must come AFTER UseRouting (implied by MapRazorComponents) and BEFORE MapRazorComponents
app.UseAuthentication(); // Enables authentication middleware
app.UseAuthorization();  // Enables authorization middleware

app.UseAntiforgery(); // Ensure this is still after UseRouting and before MapRazorComponents

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Optional: Map a fallback route for handling access denied, if you use AccessDeniedPath
// app.MapGet("/AccessDenied", (HttpContext context) => Results.Challenge(properties: new AuthenticationProperties() { RedirectUri = "/login" }));


app.Run();