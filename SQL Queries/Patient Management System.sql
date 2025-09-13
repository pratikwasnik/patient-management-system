-- 1. Create Database
CREATE DATABASE "PatientManagementSystem";
GO

-- 2. Use the newly created database
USE "PatientManagementSystem";
GO

-- 3. Create Patients Table
CREATE TABLE Patients (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing Primary Key
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DOB DATE NOT NULL,
    Gender CHAR(1) CHECK (Gender IN ('M', 'F', 'O')), -- Gender constraint (M=Male, F=Female, O=Other)
    City VARCHAR(100),
    Email VARCHAR(100) UNIQUE NOT NULL, -- Unique constraint on Email
    Phone VARCHAR(15)
);

-- 4. Create Conditions Table
CREATE TABLE Conditions (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing Primary Key
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(1000)
);

-- 5. Create PatientConditions Table
CREATE TABLE PatientConditions (
    PatientId INT, -- Foreign Key to Patients table
    ConditionId INT, -- Foreign Key to Conditions table
    DiagnosedDate DATE NOT NULL,
    PRIMARY KEY (PatientId, ConditionId), -- Composite Primary Key
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE, -- Foreign Key with CASCADE on delete
    FOREIGN KEY (ConditionId) REFERENCES Conditions(Id) ON DELETE CASCADE -- Foreign Key with CASCADE on delete
);

-- 6. Create Non-Clustered Index on the Email Column
CREATE NONCLUSTERED INDEX IDX_Patient_Email ON Patients(Email);


-- 7. Create Non-Clustered Index on the City Column in Patients Table (assuming City-based searches are frequent)
CREATE NONCLUSTERED INDEX IDX_Patient_City ON Patients(City);

-- 8. Create Non-Clustered Index on the Name Column in Conditions Table (for fast searching by condition name)
CREATE NONCLUSTERED INDEX IDX_Condition_Name ON Conditions(Name);

