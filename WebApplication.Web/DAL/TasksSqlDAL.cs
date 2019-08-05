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

        public bool CreateNewTask(Tasks task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Jobtable (Id, title) VALUES(@id, @title);", connection);


                    command.Parameters.AddWithValue("@TaskId", task.TaskId);
                    command.Parameters.AddWithValue("@JobId", task.JobId);
                    command.Parameters.AddWithValue("@Location", task.Location);
                    command.Parameters.AddWithValue("@Description", task.Description);

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
    }
}


//if (task.TaskId == null || task.Description == null || task.JobId == null || task.Location == null)
//                    {
//                        return false;
//                    }
    

