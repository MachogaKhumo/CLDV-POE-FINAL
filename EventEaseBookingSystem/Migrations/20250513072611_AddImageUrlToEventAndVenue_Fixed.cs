using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEaseBookingSystem.Migrations
{
    public partial class AddImageUrlToEventAndVenue_Fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename tables
            migrationBuilder.RenameTable(
                name: "Venues",
                newName: "Venue");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_VenueId",
                table: "Booking",
                newName: "IX_Booking_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_EventId",
                table: "Booking",
                newName: "IX_Booking_EventId");

            // Rename Events to Event (if needed)
            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameIndex(
                name: "IX_Events_VenueId",
                table: "Event",
                newName: "IX_Event_VenueId");

            // Add ImageUrl column to Event table
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);

            // Add primary keys (after renaming)
            migrationBuilder.AddPrimaryKey(
                name: "PK_Venue",
                table: "Venue",
                column: "VenueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "EventId");

            // Add foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Event_EventId",
                table: "Booking",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Venue_VenueId",
                table: "Booking",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Event_EventId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Venue_VenueId",
                table: "Booking");

            // Drop primary keys
            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venue",
                table: "Venue");

            // Remove ImageUrl column
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Event");

            // Rename back tables and indexes
            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameTable(
                name: "Venue",
                newName: "Venues");

            migrationBuilder.RenameIndex(
                name: "IX_Event_VenueId",
                table: "Events",
                newName: "IX_Events_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_EventId",
                table: "Bookings",
                newName: "IX_Bookings_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_VenueId",
                table: "Bookings",
                newName: "IX_Bookings_VenueId");

            // Add back primary keys
            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venues",
                table: "Venues",
                column: "VenueId");

            // Add back foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EventId",
                table: "Bookings",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Venues_VenueId",
                table: "Bookings",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
