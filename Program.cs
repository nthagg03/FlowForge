using FlowForge.Data;
using FlowForge.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FlowForge.Repositories;
using FlowForge.Repositories.Interfaces;
using FlowForge.Services;
using FlowForge.Services.ExecutionEngine;
using FlowForge.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<ITriggerRepository, TriggerRepository>();
builder.Services.AddScoped<IExecutionRepository, ExecutionRepository>();

// Services
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IStepService, StepService>();
builder.Services.AddScoped<ITriggerService, TriggerService>();
builder.Services.AddScoped<IWorkflowExecutor, WorkflowExecutor>();

// Action handlers (singleton — stateless, no DB access)
builder.Services.AddSingleton<IActionHandler, WriteLogActionHandler>();
builder.Services.AddSingleton<IActionHandler, SendEmailActionHandler>();
builder.Services.AddSingleton<IActionHandlerResolver, ActionHandlerResolver>();

// TimeProvider — use system clock; can be replaced in tests
builder.Services.AddSingleton(TimeProvider.System);

// Error handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

var app = builder.Build();

// Eagerly resolve ActionHandlerResolver to detect duplicate registrations at startup.
app.Services.GetRequiredService<IActionHandlerResolver>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
