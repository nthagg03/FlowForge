using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowForge.Migrations
{
    /// <inheritdoc />
    public partial class AddStepAndTriggerRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "StepExecutionLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_WorkflowId",
                table: "Triggers",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_WorkflowId_Order",
                table: "Steps",
                columns: new[] { "WorkflowId", "Order" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Workflows_WorkflowId",
                table: "Steps",
                column: "WorkflowId",
                principalTable: "Workflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_Workflows_WorkflowId",
                table: "Triggers",
                column: "WorkflowId",
                principalTable: "Workflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Workflows_WorkflowId",
                table: "Steps");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_Workflows_WorkflowId",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_WorkflowId",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Steps_WorkflowId_Order",
                table: "Steps");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "StepExecutionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
