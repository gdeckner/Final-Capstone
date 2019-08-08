using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class HoursSqlDAL : IHoursDAL
    {
        private readonly string connectionString;

        public HoursSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool CreateNewHours(Hours hour)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Hours (UserId, TaskID, TimeInHours, dateLogged) VALUES(@UserId, @TaskId, @TimeInHours, @Date);", connection);


                    command.Parameters.AddWithValue("@UserId", hour.UserId);
                    command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    command.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    command.Parameters.AddWithValue("@Date", hour.Date);


                    command.ExecuteNonQuery();

                    if (hour.UserId == null || hour.TaskId == null || hour.TimeInHours == null || hour.Date == null)
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
    }
}
