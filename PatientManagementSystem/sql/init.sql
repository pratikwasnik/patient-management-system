-- Create database and tables for Patient Management System
CREATE DATABASE PatientManagementSystem;
GO
USE PatientManagementSystem;
GO

CREATE TABLE Patients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DOB DATE NOT NULL,
    Gender CHAR(1) CHECK (Gender IN ('M', 'F', 'O')),
    City VARCHAR(100),
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(15)
);

CREATE TABLE Conditions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(1000)
);

CREATE TABLE PatientConditions (
    PatientId INT NOT NULL,
    ConditionId INT NOT NULL,
    DiagnosedDate DATE NOT NULL,
    PRIMARY KEY (PatientId, ConditionId),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE,
    FOREIGN KEY (ConditionId) REFERENCES Conditions(Id) ON DELETE CASCADE
);

-- Create non-clustered indexes
CREATE NONCLUSTERED INDEX IDX_Patient_Email ON Patients(Email);
CREATE NONCLUSTERED INDEX IDX_Patient_City ON Patients(City);
CREATE NONCLUSTERED INDEX IDX_Condition_Name ON Conditions(Name);
