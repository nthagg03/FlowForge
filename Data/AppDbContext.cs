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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Workflow>()
            .HasMany(workflow => workflow.Steps)
            .WithOne(step => step.Workflow)
            .HasForeignKey(step => step.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Workflow>()
            .HasMany(workflow => workflow.Triggers)
            .WithOne(trigger => trigger.Workflow)
            .HasForeignKey(trigger => trigger.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Step>()
            .HasIndex(step => new { step.WorkflowId, step.Order })
            .IsUnique();
    }
}
