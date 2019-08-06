
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
	roles_Id int identity (1,1) primary key,
	roles_Title varchar (50) unique,
	role_Description varchar(250)
)
Create table UserLogin
(
	userID int identity (1,1) primary key,
	first_Last_Name varchar (50) not null,
	userName varchar (50) Unique,
	userRole int foreign key references Roles(roles_ID),
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
	project_Task_Description varchar(max),
	job_Id int foreign key references Jobs(job_Id),
	location_Id int foreign key references Locations(location_Id)
	

)
Create table userJob
(
userID int foreign key references UserLogin(userId),
job_Id int foreign key references Jobs(job_Id)
)
