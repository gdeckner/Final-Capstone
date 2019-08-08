 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class TasksSqlDAL : ITaskDAL
    {
        private readonly string connectionString;

        public TasksSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Tasks> GetAllTasks(int userid)
        {
            IList<Tasks> taskList = new List<Tasks>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT project_Task_ID, job_Id FROM tasks
                                                    INNER JOIN userJob
                                                    ON tasks.job_Id = userJob.job_Id
                                                    WHERE userJob.job_id = @userId;", connection);
                command.Parameters.AddWithValue("@userid", userid);
                SqlDataReader reader = command.ExecuteReader();

                taskList = MapTaskReader(reader);
            }
            return taskList;
        }


        public bool CreateNewTask(Tasks task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Tasks (project_Task_Title, project_Task_Description, job_Id, location_Id) VALUES(@Title, @Description, @JobId, @Location);", connection);

                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@JobId", task.JobId);
                    command.Parameters.AddWithValue("@Location", task.LocationId);

                    command.ExecuteNonQuery();

                    foreach (var propertyInfo in task.GetType()
                                .GetProperties(
                                        BindingFlags.Public
                                        | BindingFlags.Instance))
                    {
                        if (propertyInfo == null)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool DeleteTask(Tasks task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM Tasks WHERE project_Task_Title = @title;", connection);

                    cmd.Parameters.AddWithValue("@title", task.Title);

                    cmd.ExecuteNonQuery();

                    if (task.Title == null)
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

        private List<Tasks> MapTaskReader(SqlDataReader reader)
        {
            List<Tasks> tasks = new List<Tasks>();

            while (reader.Read())
            {
                Tasks task = new Tasks
                {
                    TaskId = Convert.ToInt32(reader["taskId"]),
                    JobId = Convert.ToInt32(reader["jobId"]),
                };

                tasks.Add(task);
            }
            return tasks;
        }
    }
}


//if (task.TaskId == null || task.Description == null || task.JobId == null || task.Location == null)
//                    {
//                        return false;
//                    }
    

