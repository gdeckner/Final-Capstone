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

    public class JobSQLDalTests : TimeClockDataBaseTest
    {
        private JobSqlDAL dao;
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new JobSqlDAL(ConnectionString);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"insert into Jobs (job_title) Values (@title)";
                cmd.Parameters.AddWithValue("@title", "President of the United States");
                cmd.ExecuteNonQuery();
            }
        }
 
        [TestMethod]
        public void CreateJobTests()
        {
            Job testJob = new Job
            {
                Title = "Vice President"
            };
            dao.CreateNewJob(testJob);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select job_Id,job_Title
                                    from Jobs
                                     where job_Title = 'Vice President'";
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    testJob.JobId = (int)reader["job_Id"];
                    testJob.Title = (string)reader["job_Title"];
                }
            }

            Assert.AreEqual("Vice President", testJob.Title);
            Assert.AreNotEqual("President", testJob.Title);

        }
        
    }
}
