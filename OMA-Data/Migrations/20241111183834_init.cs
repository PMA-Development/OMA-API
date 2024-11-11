﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMA_Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Islands",
                columns: table => new
                {
                    IslandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Islands", x => x.IslandID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "AlarmsConfig",
                columns: table => new
                {
                    AlarmConfigID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinTemperature = table.Column<int>(type: "int", nullable: false),
                    MaxTemperature = table.Column<int>(type: "int", nullable: false),
                    MinHumidity = table.Column<int>(type: "int", nullable: false),
                    MaxHumidity = table.Column<int>(type: "int", nullable: false),
                    MinAirPressure = table.Column<int>(type: "int", nullable: false),
                    MaxAirPressure = table.Column<int>(type: "int", nullable: false),
                    IslandFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmsConfig", x => x.AlarmConfigID);
                    table.ForeignKey(
                        name: "FK_AlarmsConfig_Islands_IslandFK",
                        column: x => x.IslandFK,
                        principalTable: "Islands",
                        principalColumn: "IslandID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turbine",
                columns: table => new
                {
                    TurbineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurbineFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turbine", x => x.TurbineID);
                    table.ForeignKey(
                        name: "FK_Turbine_Islands_TurbineFK",
                        column: x => x.TurbineFK,
                        principalTable: "Islands",
                        principalColumn: "IslandID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserFK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_Logs_User_UserFK",
                        column: x => x.UserFK,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    AlarmID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IslandFK = table.Column<int>(type: "int", nullable: false),
                    TurbineFK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => x.AlarmID);
                    table.ForeignKey(
                        name: "FK_Alarms_Islands_IslandFK",
                        column: x => x.IslandFK,
                        principalTable: "Islands",
                        principalColumn: "IslandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alarms_Turbine_TurbineFK",
                        column: x => x.TurbineFK,
                        principalTable: "Turbine",
                        principalColumn: "TurbineID");
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    DeviceFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Device_Turbine_DeviceFK",
                        column: x => x.DeviceFK,
                        principalTable: "Turbine",
                        principalColumn: "TurbineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinishDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerFK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserFK = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TurbineFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_Tasks_Turbine_TurbineFK",
                        column: x => x.TurbineFK,
                        principalTable: "Turbine",
                        principalColumn: "TurbineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_User_OwnerFK",
                        column: x => x.OwnerFK,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_User_UserFK",
                        column: x => x.UserFK,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "DeviceAction",
                columns: table => new
                {
                    DeviceActionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceAction", x => x.DeviceActionID);
                    table.ForeignKey(
                        name: "FK_DeviceAction_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceData",
                columns: table => new
                {
                    DeviceDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceData", x => x.DeviceDataID);
                    table.ForeignKey(
                        name: "FK_DeviceData_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drones",
                columns: table => new
                {
                    DroneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    TaskFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drones", x => x.DroneID);
                    table.ForeignKey(
                        name: "FK_Drones_Tasks_TaskFK",
                        column: x => x.TaskFK,
                        principalTable: "Tasks",
                        principalColumn: "TaskID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    AttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.AttributeID);
                    table.ForeignKey(
                        name: "FK_Attributes_DeviceData_DeviceDataID",
                        column: x => x.DeviceDataID,
                        principalTable: "DeviceData",
                        principalColumn: "DeviceDataID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_IslandFK",
                table: "Alarms",
                column: "IslandFK");

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_TurbineFK",
                table: "Alarms",
                column: "TurbineFK");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmsConfig_IslandFK",
                table: "AlarmsConfig",
                column: "IslandFK");

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_DeviceDataID",
                table: "Attributes",
                column: "DeviceDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceFK",
                table: "Device",
                column: "DeviceFK");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceAction_DeviceId",
                table: "DeviceAction",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceData_DeviceId",
                table: "DeviceData",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Drones_TaskFK",
                table: "Drones",
                column: "TaskFK");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserFK",
                table: "Logs",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerFK",
                table: "Tasks",
                column: "OwnerFK");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TurbineFK",
                table: "Tasks",
                column: "TurbineFK");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserFK",
                table: "Tasks",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Turbine_TurbineFK",
                table: "Turbine",
                column: "TurbineFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "AlarmsConfig");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "DeviceAction");

            migrationBuilder.DropTable(
                name: "Drones");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "DeviceData");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Turbine");

            migrationBuilder.DropTable(
                name: "Islands");
        }
    }
}