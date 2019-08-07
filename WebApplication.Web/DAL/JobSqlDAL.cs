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

                    SqlCommand command = new SqlCommand(@"INSERT INTO Jobs (job_title) VALUES(@title);", connection);

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

                    SqlCommand command = new SqlCommand(@"INSERT INTO userJob (Id, title) VALUES(@id, @title);", connection);


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

        public bool DeleteJob(Job job)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM Jobs WHERE job_title = @title;", connection);

                    cmd.Parameters.AddWithValue("@title", job.Title);

                    cmd.ExecuteNonQuery();

                    if (job.Title == null)
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
                Console.Write(E);
                throw;
            }

        }

        public bool DeleteUserFromJob(UserJob assigned)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"DELETE FROM  (Id, title) VALUES(@id, @title);", connection);


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
