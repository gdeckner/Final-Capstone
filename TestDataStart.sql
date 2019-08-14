insert into Roles (roles_Title,role_Description) values ('Admin','Admin control' ),('Users','Generic User'),('Deactivated Account','Deleted Account')

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
,('Georg Deckner','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','gdeckner','Admin'),('Jack Bird','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','JBird','Admin')
,('Noah Myers','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','nmyers','Admin'),('Jason Homan','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','jhoman','Admin')

,('Jane Doe','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','jdoe','Users'),
('John Smith','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','jsmith','Users'),
('Keanu Reeves','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','kreeves','Users'),
('CD Redd','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','credd','Users'),
('James Smith','RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs','RrQlUO2CbmowsGDSpRhXZA==','jsmith2','Users')

Insert into Jobs(job_Title) values ('QA Tester'),('Data Anaylst'),('Junior Software Developer'),('Senior Software Developer'),('Scrum Master'),('Customer Support')
Insert into userJob (userID,job_Id) values (07,01),(07,02),(08,03),(09,04),(10,1),(10,2)


insert into Tasks (job_Id,project_Task_Title) values (01,'Fujitsu POS'), (01,'SSE Mobile'),(01,'SHS')
,(02,'Payments'),(02,'Shipping'),(02,'Marketing'),(03,'Customer Support Applications'),(03,'Website Team'),(04,'Mentoring'),(04,'New Application Development')
insert into Hours (dateLogged,dateWorked,task_Title,timeInHours,userID,description,taskId) values
('07-07-2019','07-01-2019','Fujitsu POS',13,07,'Tested version 11.0 of pos',01),
('07-10-2019','07-11-2019','Fujitsu POS',32,07,'Tested version 12.0 of pos',01),
('08-15-2019','08-12-2019','Fujitsu POS',07,07,'Tested version 13.0 of pos',01),
('08-14-2019','08-14-2019','SSE Mobile',13,07,'Manually tested new app',01),

('06-07-2019','06-01-2019','Customer Support Applications',7,08,'worked  mobile support',3),
('06-16-2019','06-15-2019','Website Team',45,08,'Front end development',3),
('08-02-2019','08-01-2019','Customer Support Applications',2,08,'worked  mobile support',3),
('08-14-2019','08-14-2019','Website Team',0,08,'Front end development',3),

('07-07-2019','07-01-2019','Fujitsu POS',13,09,'Tested version 11.0 of pos',01),
('07-07-2019','07-01-2019','Fujitsu POS',13,09,'Tested version 11.0 of pos',01),
('07-07-2019','07-01-2019','Fujitsu POS',13,09,'Tested version 11.0 of pos',01),
('07-07-2019','07-01-2019','Fujitsu POS',13,09,'Tested version 11.0 of pos',01),

('07-07-2019','07-01-2019','Fujitsu POS',13,10,'Tested version 11.0 of pos',01),
('07-07-2019','07-01-2019','Fujitsu POS',13,10,'Tested version 11.0 of pos',01),
('07-07-2019','07-01-2019','Fujitsu POS',13,10,'Tested version 11.0 of pos',01),
('07-07-2019','07-01-2019','Fujitsu POS',13,10,'Tested version 11.0 of pos',01)


insert into Payroll (startDate,endDate,isApproved,isSubmitted,userId) values
('07-01-2019','07-15-2019',0,1,7),
('08-01-2019','08-16-2019',0,0,7)

,('06-01-2019','06-16-2019',1,1,8)
,('08-01-2019','08-16-2019',0,0,8)

