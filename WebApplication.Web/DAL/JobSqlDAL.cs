using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class JobSqlDAL : IJobDAL
    {
        private readonly string connectionString;

        public JobSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public bool CreateNewJob(Job job)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Jobtable (Id, title) VALUES(@id, @title);", connection);


                    command.Parameters.AddWithValue("@Id", job.JobId);
                    command.Parameters.AddWithValue("@title", job.Title);

                    command.ExecuteNonQuery();

                    if (job.JobId == null || job.Title == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool AssignUserToJob(UserJob assigned)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO UserJobTable (Id, title) VALUES(@id, @title);", connection);


                    command.Parameters.AddWithValue("@JobId", assigned.JobId);
                    command.Parameters.AddWithValue("@UserId", assigned.UserId);

                    command.ExecuteNonQuery();

                    if (assigned.JobId == null || assigned.UserId == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SqlException E)
            {
                Console.WriteLine(E.Message);
                return false;
            }

        }
    }
}
