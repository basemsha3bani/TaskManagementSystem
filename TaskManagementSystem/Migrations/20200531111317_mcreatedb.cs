using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Migrations
{
    public partial class mcreatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualEndingDate = table.Column<DateTime>(nullable: false),
                    Closed = table.Column<bool>(nullable: false),
                    EndingDate = table.Column<DateTime>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    StartingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ActualEndDate = table.Column<DateTime>(nullable: true),
                    ActualStartDate = table.Column<DateTime>(nullable: true),
                    Closed = table.Column<bool>(nullable: false),
                    Employee_AssignedBy = table.Column<string>(nullable: true),
                    Employee_AssignedTo = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Project_Id = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    TaskName = table.Column<string>(nullable: false),
                    dummy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Employee_Employee_AssignedBy",
                        column: x => x.Employee_AssignedBy,
                        principalTable: "Employee",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Employee_Employee_AssignedTo",
                        column: x => x.Employee_AssignedTo,
                        principalTable: "Employee",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_Project_Id",
                        column: x => x.Project_Id,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommentAttachment = table.Column<string>(nullable: true),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    CommentEmployeeId = table.Column<string>(nullable: true),
                    CommentTaskId = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Tasks_CommentTaskId",
                        column: x => x.CommentTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentTaskId",
                table: "Comments",
                column: "CommentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Employee_AssignedBy",
                table: "Tasks",
                column: "Employee_AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Employee_AssignedTo",
                table: "Tasks",
                column: "Employee_AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Project_Id",
                table: "Tasks",
                column: "Project_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
