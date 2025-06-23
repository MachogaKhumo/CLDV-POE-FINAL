USE EventEaseDB;
GO

CREATE TABLE Venue(
    VenueId INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    VenueName VARCHAR (50) NOT NULL,
    Location VARCHAR (50) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl VARCHAR (MAX)
);

CREATE TABLE Event(
    EventId INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    EventName VARCHAR (50) NOT NULL,
    EventDate DATE NOT NULL,
    Description VARCHAR (MAX) NOT NULL,
    VenueId INT NOT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venue (VenueId)
);

CREATE TABLE Booking(
    BookingId INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    EventId INT NOT NULL,
    VenueId INT NOT NULL,
    BookingDate DATE NOT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venue (VenueId),
    FOREIGN KEY (EventId) REFERENCES Event (EventId)
);