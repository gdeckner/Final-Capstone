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

                    SqlCommand command = new SqlCommand(@"INSERT INTO userJob (userID, job_Id) VALUES(@userId, @jobId);", connection);

                    command.Parameters.AddWithValue("@userId", assigned.UserId);
                    command.Parameters.AddWithValue("@jobId", assigned.JobId);

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

        public List<Job> GetJobList()
        {

            List<Job> jobs = new List<Job>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"SELECT * FROM Jobs", connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        jobs.Add(MapRowToJob(reader));
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }


            return jobs;
        }

        public bool CreateNewTask(Tasks task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Tasks (project_Task_Title, job_Id) VALUES (@title, @id);", connection);

                    command.Parameters.AddWithValue("@title", task.Title);
                    command.Parameters.AddWithValue("@id", task.JobId);

                    command.ExecuteNonQuery();

                    if (task.Title == null || task.JobId == null)
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

        public Job MapRowToJob(SqlDataReader reader)
        {
            Job job = new Job();
            job.JobId = Convert.ToInt32(reader["job_Id"]);
            job.Title = Convert.ToString(reader["job_Title"]);

            return job;
        }

    }
}
