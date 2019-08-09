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
   public  class ProjectTasksTests : TimeClockDataBaseTest
    {
        private TasksSqlDAL dao;
        public int JobId;
        public int LocationId;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new TasksSqlDAL(ConnectionString);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"insert into Locations (location_Title,location_Description) Values ('Tech Elevator','tech Space')
                    select Scope_Identity()";
                LocationId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = @"insert into Jobs (job_Title) values('Student')
                    select Scope_Identity()";
                JobId = Convert.ToInt32(cmd.ExecuteScalar());

            }
        }
        [TestMethod]
        public void CreateProjectTasksTest()
        {
            Tasks testTask = new Tasks
            {
                Description = "New Test Description",
                JobId = JobId,
                LocationId = LocationId,
                Title = ".Net Student"
            };

            dao.CreateNewTask(testTask);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from Tasks where job_ID = @jobId";
                cmd.Parameters.AddWithValue("@jobId", JobId);

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    
                    testTask.Title = (string) reader["project_Task_Title"];
                    testTask.JobId = (int) reader["job_Id"];
                    testTask.TaskId = (int) reader["project_Task_ID"];
                }
            }

            Assert.AreEqual(JobId, testTask.JobId);;
            Assert.AreEqual(".Net Student", testTask.Title);
        }
    }

}
