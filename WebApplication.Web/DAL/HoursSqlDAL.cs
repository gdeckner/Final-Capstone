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
        private DateTime current = DateTime.Today;
        private DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);

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

                    SqlCommand command = new SqlCommand(@"INSERT INTO Hours (UserId, TaskID, TimeInHours, dateLogged, description, location) VALUES(@UserId, @TaskId, @TimeInHours, @Date, @Description, @Location);", connection);


                    command.Parameters.AddWithValue("@UserId", hour.UserId);
                    command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    command.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    command.Parameters.AddWithValue("@Date", hour.Date);
                    command.Parameters.AddWithValue("@Description", hour.Description);
                    command.Parameters.AddWithValue("@Location", hour.Location);

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

        public IList<Hours> GetAllHours(int userId)
        {
            IList<Hours> defaultHoursList = new List<Hours>();
            IList<Hours> specificHoursList = new List<Hours>();

            if (false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand(@"SELECT userID, taskId, timeInHours, dateLogged, description, location FROM Hours
                                                    WHERE userJob.userID = @userId;", connection);

                    command.Parameters.AddWithValue("@userid", userId);
                    SqlDataReader reader = command.ExecuteReader();

                    defaultHoursList = MapHoursToReader(reader);
                }
                return defaultHoursList;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand(@"SELECT userID, taskId, timeInHours, dateLogged, description, location FROM Hours
                                                    WHERE userJob.userID = @userId
                                                    AND dateLogged BETWEEN CONVERT(datetime, @thirtyDays) AND CONVERT(datetime, @currentDays);", connection);

                    command.Parameters.AddWithValue("@userid", userId);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@thirtyDays", thirtyDaysAgo);


                    SqlDataReader reader = command.ExecuteReader();

                    specificHoursList = MapHoursToReader(reader);
                }
                return specificHoursList;
            }
        }

        private List<Hours> MapHoursToReader(SqlDataReader reader)
        {
            List<Hours> hours = new List<Hours>();

            while (reader.Read())
            {
                Hours hour = new Hours
                {
                    UserId = Convert.ToInt32(reader["userID"]),
                    TaskId = Convert.ToInt32(reader["taskId"]),
                    TimeInHours = Convert.ToInt32(reader["timeInHours"]),
                    Date = Convert.ToDateTime(reader["dateLogged"]),
                    Description = Convert.ToString(reader["description"]),
                    Location = Convert.ToString(reader["location"]),
                };

                hours.Add(hour);
            }
            return hours;
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
