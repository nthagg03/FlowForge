using Microsoft.EntityFrameworkCore;
using FlowForge.Models.Entities;


namespace FlowForge.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Workflow> Workflows {get; set;}
    public DbSet<Trigger> Triggers {get; set;}
    public DbSet<Step> Steps {get; set;}
    public DbSet<WorkflowExecution> WorkflowExecutions {get; set;}
    public DbSet<StepExecutionLog> StepExecutionLogs {get; set;}
}