using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OMA_Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turbine_Islands_TurbineFK",
                table: "Turbine");

            migrationBuilder.RenameColumn(
                name: "TurbineFK",
                table: "Turbine",
                newName: "IslandFK");

            migrationBuilder.RenameIndex(
                name: "IX_Turbine_TurbineFK",
                table: "Turbine",
                newName: "IX_Turbine_IslandFK");

            migrationBuilder.InsertData(
                table: "Islands",
                columns: new[] { "IslandID", "Abbreviation", "ClientID", "Title" },
                values: new object[,]
                {
                    { 1, "IS1", "ClientA", "Island One" },
                    { 2, "IS2", "ClientB", "Island Two" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Email", "FullName", "Phone" },
                values: new object[,]
                {
                    { new Guid("c6936336-4a10-4445-b373-60f6a37a58c4"), "admin@example.com", "Admin User", "1234567890" },
                    { new Guid("cf9844c4-55aa-4eef-bba2-9b97771a8c29"), "hotlineuser@example.com", "Hotline User", "0987654321" }
                });

            migrationBuilder.InsertData(
                table: "AlarmsConfig",
                columns: new[] { "AlarmConfigID", "IslandFK", "MaxAirPressure", "MaxHumidity", "MaxTemperature", "MinAirPressure", "MinHumidity", "MinTemperature" },
                values: new object[,]
                {
                    { 1, 1, 1050, 70, 30, 1000, 30, 15 },
                    { 2, 2, 1050, 70, 30, 1000, 30, 15 }
                });

            migrationBuilder.InsertData(
                table: "Turbine",
                columns: new[] { "TurbineID", "ClientID", "IslandFK", "Title" },
                values: new object[,]
                {
                    { 1, "ClientA", 1, "Turbine 1 - Island 1" },
                    { 2, "ClientA", 1, "Turbine 2 - Island 1" },
                    { 3, "ClientA", 1, "Turbine 3 - Island 1" },
                    { 4, "ClientA", 1, "Turbine 4 - Island 1" },
                    { 5, "ClientA", 1, "Turbine 5 - Island 1" },
                    { 6, "ClientB", 2, "Turbine 1 - Island 2" },
                    { 7, "ClientB", 2, "Turbine 2 - Island 2" },
                    { 8, "ClientB", 2, "Turbine 3 - Island 2" },
                    { 9, "ClientB", 2, "Turbine 4 - Island 2" },
                    { 10, "ClientB", 2, "Turbine 5 - Island 2" }
                });

            migrationBuilder.InsertData(
                table: "Alarms",
                columns: new[] { "AlarmID", "IslandFK", "TurbineFK" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskID", "Description", "FinishDescription", "OwnerFK", "Title", "TurbineFK", "Type", "UserFK" },
                values: new object[,]
                {
                    { 1, "Description for Task One", "Finish Task One", new Guid("c6936336-4a10-4445-b373-60f6a37a58c4"), "Task One", 1, "Type A", null },
                    { 2, "Description for Task Two", "Finish Task Two", new Guid("cf9844c4-55aa-4eef-bba2-9b97771a8c29"), "Task Two", 6, "Type B", null }
                });

            migrationBuilder.InsertData(
                table: "Drones",
                columns: new[] { "DroneID", "Available", "TaskFK", "Title" },
                values: new object[,]
                {
                    { 1, true, 1, "Drone One" },
                    { 2, false, 2, "Drone Two" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Turbine_Islands_IslandFK",
                table: "Turbine",
                column: "IslandFK",
                principalTable: "Islands",
                principalColumn: "IslandID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turbine_Islands_IslandFK",
                table: "Turbine");

            migrationBuilder.DeleteData(
                table: "Alarms",
                keyColumn: "AlarmID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Alarms",
                keyColumn: "AlarmID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AlarmsConfig",
                keyColumn: "AlarmConfigID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AlarmsConfig",
                keyColumn: "AlarmConfigID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Drones",
                keyColumn: "DroneID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drones",
                keyColumn: "DroneID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "TaskID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "TaskID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Turbine",
                keyColumn: "TurbineID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserID",
                keyValue: new Guid("c6936336-4a10-4445-b373-60f6a37a58c4"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserID",
                keyValue: new Guid("cf9844c4-55aa-4eef-bba2-9b97771a8c29"));

            migrationBuilder.DeleteData(
                table: "Islands",
                keyColumn: "IslandID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Islands",
                keyColumn: "IslandID",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "IslandFK",
                table: "Turbine",
                newName: "TurbineFK");

            migrationBuilder.RenameIndex(
                name: "IX_Turbine_IslandFK",
                table: "Turbine",
                newName: "IX_Turbine_TurbineFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Turbine_Islands_TurbineFK",
                table: "Turbine",
                column: "TurbineFK",
                principalTable: "Islands",
                principalColumn: "IslandID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
