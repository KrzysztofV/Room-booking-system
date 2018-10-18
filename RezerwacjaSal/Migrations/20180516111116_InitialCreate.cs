using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RezerwacjaSal.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Administrator = table.Column<int>(nullable: false),
                    Manager = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Pearson",
                columns: table => new
                {
                    PearsonID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Employee = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    PearsonNumber = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pearson", x => x.PearsonID);
                });

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    BuildingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 50, nullable: false),
                    DepartmentID = table.Column<int>(nullable: false),
                    GPS_E = table.Column<string>(nullable: true),
                    GPS_N = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.BuildingID);
                    table.ForeignKey(
                        name: "FK_Building_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employment",
                columns: table => new
                {
                    EmploymentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentID = table.Column<int>(nullable: true),
                    PearsonID = table.Column<int>(nullable: false),
                    Position = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employment", x => x.EmploymentID);
                    table.ForeignKey(
                        name: "FK_Employment_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employment_Pearson_PearsonID",
                        column: x => x.PearsonID,
                        principalTable: "Pearson",
                        principalColumn: "PearsonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    MessageContent = table.Column<string>(nullable: false),
                    MessageSubject = table.Column<string>(nullable: false),
                    PearsonID = table.Column<int>(nullable: true),
                    PearsonName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Message_Pearson_PearsonID",
                        column: x => x.PearsonID,
                        principalTable: "Pearson",
                        principalColumn: "PearsonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuildingID = table.Column<int>(nullable: false),
                    Equipment = table.Column<string>(maxLength: 100, nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Spots = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Room_Building_BuildingID",
                        column: x => x.BuildingID,
                        principalTable: "Building",
                        principalColumn: "BuildingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    PearsonID = table.Column<int>(nullable: false),
                    RoomID = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservation_Pearson_PearsonID",
                        column: x => x.PearsonID,
                        principalTable: "Pearson",
                        principalColumn: "PearsonID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Room_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Room",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Building_DepartmentID",
                table: "Building",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Employment_DepartmentID",
                table: "Employment",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Employment_PearsonID",
                table: "Employment",
                column: "PearsonID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_PearsonID",
                table: "Message",
                column: "PearsonID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_PearsonID",
                table: "Reservation",
                column: "PearsonID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_RoomID",
                table: "Reservation",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_BuildingID",
                table: "Room",
                column: "BuildingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employment");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Pearson");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
