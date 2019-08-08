using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class HoursSQLDalTests : TimeClockDataBaseTest
    {
        private HoursSqlDAL dao;
        public int JobId;
        public int LocationId;
        public int TaskId;
        public int UserId;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new HoursSqlDAL(ConnectionString);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"insert into Roles (roles_Title,role_Description) values ('Admin','Admin control' ),('Users','Generic User')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"insert into UserLogin (first_Last_Name,userName,userRole,password,salt) values ('Gerg DinkleBerry','gdeckner','Admin',@password,@salt)";
                cmd.Parameters.AddWithValue("@salt", "RrQlUO2CbmowsGDSpRhXZA==");
                cmd.Parameters.AddWithValue("@password", "RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs");

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"insert into Locations (location_Title,location_Description) Values ('Tech Elevator','tech Space')
                    select Scope_Identity()";
                LocationId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = @"insert into Jobs (job_Title) values('Student')
                    select Scope_Identity()";
                JobId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = @"INSERT INTO Tasks (project_Task_Title, project_Task_Description, job_Id, location_Id) VALUES('PennyWiser', 'Collecting Turtles', @JobId, @Location)
                    select Scope_Identity()";
                cmd.Parameters.AddWithValue("@JobId", JobId);
                cmd.Parameters.AddWithValue("@Location", LocationId);

                TaskId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = @"select userID from userLogin where userName = 'gdeckner'";
                UserId = Convert.ToInt32(cmd.ExecuteScalar());


            }
        }

        [TestMethod]
        public void CreateHoursTest()
        {
            Hours testhours = new Hours
            {
                Date = DateTime.Today,
                TaskId = TaskId,
                TimeInHours = 7.60M,
                UserId = UserId
                

            };
            dao.CreateNewHours(testhours);

            Hours pulledHours = new Hours();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from Hours";
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    pulledHours.Date = (DateTime)reader["dateLogged"];
                    pulledHours.TaskId = (int)reader["taskId"];
                    pulledHours.UserId = (int)reader["userId"];
                    pulledHours.TimeInHours = (decimal)reader["timeInHours"];
                }
            }

            Assert.AreEqual(testhours.TaskId, pulledHours.TaskId);
            Assert.AreEqual(testhours.TimeInHours, pulledHours.TimeInHours);
        }
    }
}
