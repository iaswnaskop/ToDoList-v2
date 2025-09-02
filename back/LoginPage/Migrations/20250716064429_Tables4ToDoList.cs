using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoginPage.Migrations
{
    /// <inheritdoc />
    public partial class Tables4ToDoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("status_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    statusid = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    untilat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_status_statusid",
                        column: x => x.statusid,
                        principalTable: "status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users_tasks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_tasks", x => new { x.UserId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_users_tasks_task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_tasks_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "status",
                columns: new[] { "id", "code" },
                values: new object[,]
                {
                    { 1, "ToDo" },
                    { 2, "InProgress" },
                    { 3, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_statusid",
                table: "task",
                column: "statusid");

            migrationBuilder.CreateIndex(
                name: "IX_users_tasks_TaskId",
                table: "users_tasks",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_tasks");

            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "status");
        }
    }
}
