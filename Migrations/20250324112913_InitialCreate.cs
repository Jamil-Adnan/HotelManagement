using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraBeds = table.Column<bool>(type: "bit", nullable: false),
                    PricePerNight = table.Column<double>(type: "float", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    BookedFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookedTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false),
                    CheckedIn = table.Column<bool>(type: "bit", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "GuestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "GuestId", "Country", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "England", "ironmaiden@email.com", "Iron", "Maiden", "0044-12345678" },
                    { 2, "USA", "sixfeetunder@email.com", "Sixfeet", "Under", "001-12345678" },
                    { 3, "Finland", "insomnium@email.com", "Insomnium", "NA", "00358-123456" },
                    { 4, "Sweden", "opeth@email.com", "Opeth", "NA", "0046-123456" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Availability", "ExtraBeds", "PricePerNight", "RoomType" },
                values: new object[,]
                {
                    { 1, true, false, 399.0, "Single" },
                    { 2, false, true, 699.0, "Double" },
                    { 3, true, false, 599.0, "Double" },
                    { 4, false, false, 399.0, "Single" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookedFrom", "BookedTill", "CheckedIn", "GuestId", "Paid", "RoomId", "TotalCost" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, true, 1, 1197.0 },
                    { 2, new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, true, 2, 6291.0 },
                    { 3, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, false, 3, 4193.0 },
                    { 4, new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, true, 1, 5985.0 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "BookingId", "PaymentDate", "PaymentMethod" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card" },
                    { 2, 2, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "PayPal" },
                    { 3, 3, null, null },
                    { 4, 4, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuestId",
                table: "Bookings",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
