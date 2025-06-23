USE EventEasepoe3km;
GO

CREATE TABLE Venue(
	VenueId INT IDENTITY(1,1) PRIMARY KEY, 
	VenueName VARCHAR (250) NOT NULL,
	Location VARCHAR (250) NOT NULL,
	Capacity INT,
	ImageURL VARCHAR (250) NOT NULL
);

CREATE TABLE EventType(
	EventTypeId INT IDENTITY(1,1) PRIMARY KEY, 
	EventTypeName VARCHAR (250) NOT NULL
);

CREATE TABLE Event(
	EventId INT IDENTITY(1,1) PRIMARY KEY, 
	VenueId INT NOT NULL,
	EventTypeId INT NOT NULL,
	EventName VARCHAR (250) NOT NULL,
	EventDate DATE NOT NULL,
	Description VARCHAR (250) NOT NULL,
	CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId), 
	CONSTRAINT FK_Event_EventType FOREIGN KEY (EventTypeId) REFERENCES EventType(EventTypeId)
);

CREATE TABLE Booking(
	BookingId INT IDENTITY(1,1) PRIMARY KEY, 
	VenueId INT NOT NULL, 
	EventId INT NOT NULL, 
	BookingDate DATE NOT NULL,
	CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId), 
	CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId) 
);

--Insert sample data 
INSERT INTO Venue (VenueName,Location, Capacity, ImageURL) 
VALUES ('The Garden Venue Hotel', '308 Boundary Rd', 70, 'htps://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.facebook.com%2Fbilliardcafesa%2F&psig=AOvVaw0AImhiBzrTa5XMFFvpudRW&ust=1744116194122000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCNDhlaz5xYwDFQAAAAAdAAAAABAE');

INSERT INTO EventType (EventTypeName)
VALUES	('Conference'),
		('Wedding'),
		('Birthday'),
		('Concert'),
		('Gala');

INSERT INTO Event (VenueId, EventName,EventDate, Description, EventTypeId)
VALUES (1, 'Rens IT Conference', '2025-10-18', 'Rens wants to host an IT Conference', 5);

INSERT INTO Booking (VenueId, EventId,BookingDate)
VALUES (1, 1, '2025-10-18');