insert into Roles (roles_Title,role_Description) values ('Admin','Admin control' ),('Users','Generic User')

insert into UserLogin (first_Last_Name, userName,userRole,password,salt)
values('Default Admin', 'Admin','Admin','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==')

INSERT INTO Locations(location_Title, location_Description) VALUES('AAT_HQ', 'AwareAbility Technologies Office - Rev1');
INSERT INTO Locations(location_Title) VALUES('AAT_Working Lab');
INSERT INTO Locations(location_Title, location_Description) VALUES('OSU_LAB', 'OSU Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('UC_LAB', 'University of Cincinnati Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('NANO_LAB', 'NanoTech West Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('ARC', 'ARC Industries Office');
INSERT INTO Locations(location_Title, location_Description) VALUES('FCBDD', 'Franklin County Board of Developmental Disabilities');
INSERT INTO Locations(location_Title, location_Description) VALUES('REM', 'Remote telework from other location');

Insert into UserLogin (first_Last_Name,password,salt,userName,userRole) values ('Ted Grundy','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','tGrundy','Users')

Insert into Jobs(job_Title) values ('Protomolecule Employee'),('Burger King Helper')
Insert into userJob (userID,job_Id) values (02,01)
insert into Tasks (job_Id,project_Task_Title) values (01,'Part of the wall'), (01,'King in da norf')
insert into Hours (dateLogged,dateWorked,task_Title,timeInHours,userID,description,taskId) values ('08-01-2019','07-30-2019','Protomolecule Employee',43,02,'oh noez',01),
('08-14-2019','08-16-2019','King in da norf',20,02,'dw',02)


insert into Payroll (startDate,endDate,isApproved,isSubmitted,userId) values ('07-01-2019','07-15-2019',0,1,2),('07-16-2019','07-25-2019',1,1,2),('08-01-2019','08-15-2019',0,0,2)

