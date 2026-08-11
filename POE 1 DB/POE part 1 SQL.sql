--Create Database
CREATE DATABASE POEPART1DB;
GO
USE POEPART1DB;
GO 
--1 .ROLES
CREATE TABLE Roles(
RolesId INT IDENTITY(1,1) PRIMARY KEY, 
RolesName VARCHAR(50) NOT NULL UNIQUE
);
--2. CREATE USERS
 CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    RolesId INT NOT NULL,
    OrganiserId INT NULL,
    CONSTRAINT FK_Users_Organiser
        FOREIGN KEY (OrganiserId) REFERENCES Users(UserId)
)
 -- 3. EVENTS
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(150) NOT NULL,
    EventDate DATETIME NOT NULL,
    Location VARCHAR(150) NOT NULL,
    OrganiserId INT NOT NULL,
    CONSTRAINT FK_Events_Organiser FOREIGN KEY (OrganiserId) REFERENCES Users(UserId)
);

-- 4. Categories 
 CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL,
    EventId INT NOT NULL,
    MaxParticipant INT NULL
    CONSTRAINT FK_Category_Events FOREIGN KEY (EventId) REFERENCES Events(EventId)
);
-- 5. EventEnrollment 
CREATE TABLE EventEnrolments (
    EnrolmentId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NOT NULL,
    ParticipantId INT NOT NULL,
    EnrolmentDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Enrolments_Category FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    CONSTRAINT FK_Enrolments_Participant FOREIGN KEY (ParticipantId) REFERENCES Users(UserId),
    CONSTRAINT UQ_Participant_Category UNIQUE (CategoryId, ParticipantId)
);

-- 6. Results
CREATE TABLE Results (
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentId INT NOT NULL UNIQUE,
    FinishTime TIME NULL,
    Position INT NULL,
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentId) REFERENCES EventEnrolments(EnrolmentId)
);

 -- Insert Roles
INSERT INTO Roles (RolesName)
VALUES ('Organiser'), ('Participant');

-- Insert Users
INSERT INTO Users (FullName, Email, PasswordHash, RolesId)
VALUES
('Alice Smith', 'alice@raceday.com', 'hashed_pwd_1', 1),
('Bob Jones', 'bob@raceday.com', 'hashed_pwd_2', 1),
('Charlie Brown', 'charlie@gmail.com', 'hashed_pwd_3', 2),
('Diana Prince', 'diana@gmail.com', 'hashed_pwd_4', 2);

-- Insert Events
INSERT INTO Events (EventName, EventDate, Location, OrganiserId)
VALUES
('City Marathon 2026', '2026-09-15 06:00:00', 'Central Park', 1),
('Mountain Trail Run', '2026-10-10 07:30:00', 'Green Valley', 1),
('Coastal Cycling Tour', '2026-11-05 08:00:00', 'Ocean Drive', 2);

-- Insert Categories
INSERT INTO Categories (EventId, CategoryName, MaxParticipant)
VALUES
(1, 'Full Marathon 42km', 500),
(1, 'Half Marathon 21km', 1000),
(2, 'Trail 15km', 200),
(3, 'Cycle 50km', 300);

SELECT * FROM Categories;
SELECT * FROM Users;
SELECT * FROM Events;
SELECT * FROM Results;

-- Insert Event Enrolments
INSERT INTO EventEnrolments (CategoryId, ParticipantId)
VALUES
(1, 3),
(2, 4);

-- Insert Results
INSERT INTO Results (EnrolmentId, FinishTime, Position)
VALUES
(1, '03:45:12', 12),
(2, '01:52:04', 5);
