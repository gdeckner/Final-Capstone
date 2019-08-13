
USE master;
GO

--
IF EXISTS(select * from sys.databases where name='TimeTrackDataBase')
DROP DATABASE TimeTrackDataBase;
GO


CREATE DATABASE TimeTrackDataBase;
GO


USE TimeTrackDataBase

GO

Create table Roles
(
	roles_Title varchar (50) primary key,
	role_Description varchar(250)
)
Create table UserLogin
(
	userID int identity (1,1) primary key,
	first_Last_Name varchar (50) not null,
	userName varchar (50) Unique,
	userRole varchar (50) foreign key references Roles(roles_Title),
	password varchar(200)COLLATE Latin1_General_CI_AS not null,
	salt varchar (200) not null
)
Create table Jobs
(
	job_Id int identity (1,1) primary key,
	job_Title varchar (100)
)
Create table Locations
(
	location_Id int identity(1,1) primary key,
	location_Title varchar (50),
	location_Description varchar (max)


)
Create table Tasks
(
	project_Task_ID int identity (1,1) primary key,
	project_Task_Title varchar (100),
	job_Id int foreign key references Jobs(job_Id),
)

Create table userJob
(
userID int foreign key references UserLogin(userId),
job_Id int foreign key references Jobs(job_Id)
)
create table Hours
(
	hoursId int identity (1,1) primary key,
	userID int foreign key references UserLogin(userId),
	taskId int foreign key references Tasks(project_Task_ID),
	description varchar(max),
	location varchar(100),
	timeInHours decimal (4,2),
	dateWorked date,
	dateLogged date,
	task_Title varchar(100),
	

)

create table Payroll
(
	userId int foreign key references UserLogin(userId),
	startDate date,
	endDate date,
	isApproved bit default 0,
	isSubmitted bit default 0 not null

)
create table Log
(
	log_Id int identity(1,1) primary key,
	targetUser int foreign key references UserLogin(userId),
	dateWorked date,
	dateLogged date,
	modified_Date Date,
	hoursId int foreign key references Hours(hoursId),
	hoursBefore decimal,
	hoursAfter decimal,
	currentUser int foreign key references UserLogin(userId)
)


insert into Roles (roles_Title,role_Description) values ('Admin','Admin control' ),('Users','Generic User')
insert into UserLogin (first_Last_Name, userName,userRole,password,salt) 
values('Default Admin', 'Admin','Admin','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA=='),('Default User','User','Users','qmtnGXtPXRXBLwrAJDC7wvz3msY=','nhyQHUYxoa0='),
('Gerg Acer','GAcer','Users', 'RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==')
INSERT INTO Locations(location_Title, location_Description) VALUES('AAT_HQ', 'AwareAbility Technologies Office - Rev1');
INSERT INTO Locations(location_Title) VALUES('AAT_Working Lab');
INSERT INTO Locations(location_Title, location_Description) VALUES('OSU_LAB', 'OSU Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('UC_LAB', 'University of Cincinnati Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('NANO_LAB', 'NanoTech West Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('ARC', 'ARC Industries Office');
INSERT INTO Locations(location_Title, location_Description) VALUES('FCBDD', 'Franklin County Board of Developmental Disabilities');
INSERT INTO Locations(location_Title, location_Description) VALUES('REM', 'Remote telework from other location');

go
Create Trigger insertedHours
on Hours
After  INSERT
as
begin
	INSERT INTO LOG(
	hoursId,hoursAfter,targetUser,dateWorked,dateLogged)
	Select
	h.hoursId,
	h.timeInHours,
	h.userID,
	h.dateWorked,
	h.dateLogged
	FROM inserted h
	
	
end;
go

create trigger UpdatedHours
on hours
after update
as
begin
update log
set targetUser = i.userID,hoursAfter = i.timeInHours,
hoursId = i.hoursId,dateWorked = i.dateWorked, modified_Date = CURRENT_TIMESTAMP
from inserted i
	

end;
go

