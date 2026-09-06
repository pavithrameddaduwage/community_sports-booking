-- 1. Create Members Table
CREATE TABLE Members (
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    ContactNumber VARCHAR(20) NOT NULL,
    Address VARCHAR(200) NOT NULL,
    PreferredSports VARCHAR(200),
    Role VARCHAR(20) DEFAULT 'Member',
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 2. Create Facilities Table
CREATE TABLE Facilities (
    FacilityId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    FacilityType VARCHAR(50) NOT NULL,
    Location VARCHAR(150) NOT NULL,
    HourlyRate DECIMAL(18,2) NOT NULL,
    Capacity INT NOT NULL,
    Description VARCHAR(500),
    ImageUrl VARCHAR(255)
);

-- 3. Create Bookings Table
CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    FacilityId INT NOT NULL,
    BookingDate DATE NOT NULL,
    TimeSlot VARCHAR(50) NOT NULL,
    Status VARCHAR(30) DEFAULT 'Confirmed',
    TotalPrice DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Bookings_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId) ON DELETE CASCADE,
    CONSTRAINT FK_Bookings_Facilities FOREIGN KEY (FacilityId) REFERENCES Facilities(FacilityId) ON DELETE CASCADE
);

-- 4. Create Reviews Table
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    FacilityId INT NOT NULL,
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    Comment VARCHAR(500) NOT NULL,
    ReviewDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Facilities FOREIGN KEY (FacilityId) REFERENCES Facilities(FacilityId) ON DELETE CASCADE
);
