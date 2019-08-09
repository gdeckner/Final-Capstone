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

        public Hours PullLoggedHours(int? userId)
        {
            Hours pulledHours = new Hours();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from Hours where userId = @userId";
                cmd.Parameters.AddWithValue("@userId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pulledHours.Date = (DateTime)reader["dateLogged"];
                    pulledHours.TaskId = (int)reader["taskId"];
                    pulledHours.UserId = (int)reader["userId"];
                    pulledHours.TimeInHours = (decimal)reader["timeInHours"];
                }
            }

            return pulledHours;
        }
    }
}
