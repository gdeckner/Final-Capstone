INSERT INTO Locations(location_Title, location_Description) VALUES('AAT_HQ', 'AwareAbility Technologies Office - Rev1');
INSERT INTO Locations(location_Title) VALUES('AAT_Working Lab');
INSERT INTO Locations(location_Title, location_Description) VALUES('OSU_LAB', 'OSU Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('UC_LAB', 'University of Cincinnati Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('NANO_LAB', 'NanoTech West Laboratory');
INSERT INTO Locations(location_Title, location_Description) VALUES('ARC', 'ARC Industries Office');
INSERT INTO Locations(location_Title, location_Description) VALUES('FCBDD', 'Franklin County Board of Developmental Disabilities');
INSERT INTO Locations(location_Title, location_Description) VALUES('REM', 'Remote telework from other location');
Insert into Jobs (job_Title) Values ('Holiday'),('QA Tester'),('President')
insert into userJob (userID,job_Id) values (03,03)
Insert into Tasks (job_Id,project_Task_Title) Values (03,'Database Tester'),(03,'Watching Coder Monkey')
insert into Hours (userID,taskId,dateLogged,location,timeInHours) Values (03,01,'08/02/2019','NANO_LAB',7.8),
(03,01,'08/03/2019','NANO_LAB',8),(02,02,'08/09/2019','NANO_LAB',13)
