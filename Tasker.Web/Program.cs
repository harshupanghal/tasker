using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Tasker.Application.Interfaces;
using Tasker.Application.UseCases.Tasks;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories;
using Tasker.Infrastructure.Services;
using Tasker.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("Connection string not found");
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

// DI for Repos & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Use Cases
builder.Services.AddScoped<CreateTaskUseCase>();
builder.Services.AddScoped<GetTasksForUserUseCase>();
builder.Services.AddScoped<GetTaskByIdUseCase>();
builder.Services.AddScoped<UpdateTaskUseCase>();
builder.Services.AddScoped<DeleteTaskUseCase>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<ProtectedLocalStorage>();



// Blazor-specific
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
