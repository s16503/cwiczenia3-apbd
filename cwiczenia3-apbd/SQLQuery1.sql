

/*
-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2020-03-22 07:12:20.376

-- tables
-- Table: Enrollment
CREATE TABLE Enrollment (
    IdEnrollment int  NOT NULL,
    Semester int  NOT NULL,
    IdStudy int  NOT NULL,
    StartDate date  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY  (IdEnrollment)
);

-- Table: Student
CREATE TABLE Student (
    IndexNumber nvarchar(100)  NOT NULL,
    FirstName nvarchar(100)  NOT NULL,
    LastName nvarchar(100)  NOT NULL,
    BirthDate date  NOT NULL,
    IdEnrollment int  NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY  (IndexNumber)
);

-- Table: Studies
CREATE TABLE Studies (
    IdStudy int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY  (IdStudy)
);

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE Enrollment ADD CONSTRAINT Enrollment_Studies
    FOREIGN KEY (IdStudy)
    REFERENCES Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE Student ADD CONSTRAINT Student_Enrollment
    FOREIGN KEY (IdEnrollment)
    REFERENCES Enrollment (IdEnrollment);

-- End of file.


INSERT INTO Studies(IdStudy,Name)
VALUES (1,'Informatyka');

INSERT INTO Enrollment(IdEnrollment,Semester,  IdStudy, StartDate)
VALUES (23243,2,1,'2019-02-03');

INSERT INTO Student(IndexNumber,FirstName, lastname, birthdate, IdEnrollment)
VALUES (14233,'Jan','Kowalski','1999-02-03', 23243);

INSERT INTO Student(IndexNumber,FirstName, lastname, birthdate, IdEnrollment)
VALUES (15333,'Andrzej','Nowak','2000-02-03', 23243);

INSERT INTO Student(IndexNumber,FirstName, lastname, birthdate, IdEnrollment)
VALUES (16444,'Jan','Kowalski','1997-02-03', 23243);

SELECT * FROM student;
*/



SELECT Enrollment.IdEnrollment, Semester, Studies.Name, StartDate FROM Enrollment JOIN Student ON Student.IdEnrollment = Enrollment.IdEnrollment JOIN Studies ON Enrollment.IdStudy=Studies.IdStudy WHERE Student.IndexNumber = 14233;




    INSERT INTO Studies VALUES(3,'Architektura wnętrz');
    INSERT INTO Enrollment values(433,3,3,'2018-10-01');


    INSERT INTO Student(IndexNumber,FirstName, lastname, birthdate, IdEnrollment)
        VALUES (12320,'Aleksander','Byk','1996-02-03', 433);

      