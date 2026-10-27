using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowForge.Migrations
{
    /// <inheritdoc />
    public partial class AddExecutionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriggerType",
                table: "WorkflowExecutions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "StepExecutionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Output",
                table: "StepExecutionLogs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowExecutions_WorkflowId_StartedAt",
                table: "WorkflowExecutions",
                columns: new[] { "WorkflowId", "StartedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StepExecutionLogs_StepId",
                table: "StepExecutionLogs",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_StepExecutionLogs_WorkflowExecutionId",
                table: "StepExecutionLogs",
                column: "WorkflowExecutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepExecutionLogs_Steps_StepId",
                table: "StepExecutionLogs",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepExecutionLogs_WorkflowExecutions_WorkflowExecutionId",
                table: "StepExecutionLogs",
                column: "WorkflowExecutionId",
                principalTable: "WorkflowExecutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowExecutions_Workflows_WorkflowId",
                table: "WorkflowExecutions",
                column: "WorkflowId",
                principalTable: "Workflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepExecutionLogs_Steps_StepId",
                table: "StepExecutionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_StepExecutionLogs_WorkflowExecutions_WorkflowExecutionId",
                table: "StepExecutionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowExecutions_Workflows_WorkflowId",
                table: "WorkflowExecutions");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowExecutions_WorkflowId_StartedAt",
                table: "WorkflowExecutions");

            migrationBuilder.DropIndex(
                name: "IX_StepExecutionLogs_StepId",
                table: "StepExecutionLogs");

            migrationBuilder.DropIndex(
                name: "IX_StepExecutionLogs_WorkflowExecutionId",
                table: "StepExecutionLogs");

            migrationBuilder.DropColumn(
                name: "TriggerType",
                table: "WorkflowExecutions");

            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "StepExecutionLogs");

            migrationBuilder.DropColumn(
                name: "Output",
                table: "StepExecutionLogs");
        }
    }
}
