USE EventEaseDB;
GO

INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES ('Hall 1', 'ROOEPOORT', 100, 'https://eventcldv.blob.core.windows.net/venues/conference-hall.jpg');

-- Check the VenueId generated for the inserted Venue
SELECT * FROM Venue;

-- Insert into Event table with an existing VenueId
INSERT INTO Event(EventName, EventDate, Description, VenueId)
VALUES ('Wedding', '2025-10-02', 'Slay', 1);  -- Use valid VenueId, e.g., 1

-- Insert into Booking table with an existing EventId and VenueId
INSERT INTO Booking(EventId, VenueId, BookingDate)
VALUES (1, 1, '2025-12-08');  -- Use valid EventId and VenueId

SELECT * FROM Venue;
