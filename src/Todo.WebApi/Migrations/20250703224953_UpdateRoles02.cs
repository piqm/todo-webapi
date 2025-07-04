using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoles02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_users_reviewer_id1",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "ix_tasks_reviewer_id1",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "reviewer_id1",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_employee_id",
                table: "tasks",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_users_employee_id",
                table: "tasks",
                column: "employee_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_users_employee_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "ix_tasks_employee_id",
                table: "tasks");

            migrationBuilder.AddColumn<Guid>(
                name: "reviewer_id1",
                table: "tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_tasks_reviewer_id1",
                table: "tasks",
                column: "reviewer_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_users_reviewer_id1",
                table: "tasks",
                column: "reviewer_id1",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
