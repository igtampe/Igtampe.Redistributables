using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Toffee.Data.Migrations
{
#pragma warning disable IDE1006 // Naming Styles
    public partial class initial : Migration
#pragma warning restore IDE1006 // Naming Styles
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Image", x => x.ID));

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    ProfilePictureID = table.Column<Guid>(type: "uuid", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                    table.ForeignKey(
                        name: "FK_User_Image_ProfilePictureID",
                        column: x => x.ProfilePictureID,
                        principalTable: "Image",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    CreatorUsername = table.Column<string>(type: "text", nullable: true),
                    IconID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Category_Image_IconID",
                        column: x => x.IconID,
                        principalTable: "Image",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Category_User_CreatorUsername",
                        column: x => x.CreatorUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CategoryID = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignerUsername = table.Column<string>(type: "text", nullable: true),
                    AssigneeUsername = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StatusMessage = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Task_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Task_User_AssigneeUsername",
                        column: x => x.AssigneeUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                    table.ForeignKey(
                        name: "FK_Task_User_AssignerUsername",
                        column: x => x.AssignerUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskID = table.Column<Guid>(type: "uuid", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerUsername = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notification_Task_TaskID",
                        column: x => x.TaskID,
                        principalTable: "Task",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Notification_User_OwnerUsername",
                        column: x => x.OwnerUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatorUsername",
                table: "Category",
                column: "CreatorUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Category_IconID",
                table: "Category",
                column: "IconID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_OwnerUsername",
                table: "Notification",
                column: "OwnerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TaskID",
                table: "Notification",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AssigneeUsername",
                table: "Task",
                column: "AssigneeUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AssignerUsername",
                table: "Task",
                column: "AssignerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Task_CategoryID",
                table: "Task",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProfilePictureID",
                table: "User",
                column: "ProfilePictureID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Image");
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
