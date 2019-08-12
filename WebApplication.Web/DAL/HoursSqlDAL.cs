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
        private readonly DateTime current = DateTime.Today;
        private readonly DateTime lastMonth = DateTime.Now.AddDays(-30);
        private readonly DateTime lastWeek = DateTime.Now.AddDays(-7);
        private readonly DateTime lastQuarter = DateTime.Now.AddDays(-120);
        private string updateHoursQuery = @"(BEGIN TRANSACTION);";

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

                    SqlCommand command = new SqlCommand(@"INSERT INTO Hours (UserId, TaskID, TimeInHours, dateWorked, dateLogged, description, location) VALUES(@UserId, @TaskId, @TimeInHours, @WorkedDate, @LoggedDate, @Description, @Location);", connection);


                    command.Parameters.AddWithValue("@UserId", hour.UserId);
                    command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    command.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    command.Parameters.AddWithValue("@Description", hour.Description);
                    command.Parameters.AddWithValue("@Location", hour.Location);
                    command.Parameters.AddWithValue("@WorkedDate", hour.Date);
                    command.Parameters.AddWithValue("@LoggedDate", current);
                    //command.Parameters.AddWithValue("@IsSubmitted", hour.isSubmitted);



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

        public bool UpdateHours(Hours hour)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (hour.TimeInHours != null)
                    {
                        string updateTime = @"(UPDATE Hours SET timeInHours = @TimeInHours WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);";
                        updateHoursQuery += updateTime;
                    }
                    if (hour.TimeInHours != null)
                    {
                        string updateTime = @"(UPDATE Hours SET taskId = @TaskId WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);";
                        updateHoursQuery += updateTime;
                    }
                    if (hour.Description != null)
                    {
                        string updateDescription = @"(UPDATE Hours SET Description = @Description WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);";
                        updateHoursQuery += updateDescription;
                    }
                    if (hour.Description != null)
                    {
                        string updateDescription = @"(UPDATE Hours SET dateLogged = @loggedDate WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);";
                        updateHoursQuery += updateDescription;
                    }

                    string loggedDate = @"(UPDATE Hours SET dateLogged = @DateLogged WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);";
                    string commit = @"(COMMIT);";
                    updateHoursQuery += loggedDate;
                    updateHoursQuery += commit;


                    SqlCommand command = new SqlCommand(@"UPDATE Hours SET timeInHours = @TimeInHours WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);", connection);
                    SqlCommand commandTwo = new SqlCommand(@"UPDATE Hours SET dateLogged = @DateLogged WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);", connection);


                    string query = $@"{updateHoursQuery}
                                  WHERE park_id = {hour.UserId}'";

                    query = $@"{AcadiaSelectQuery} WHERE park_id = @parkId";


                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@UserId", hour.UserId);
                    command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    command.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    commandTwo.Parameters.AddWithValue("@UserId", hour.UserId);
                    commandTwo.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    commandTwo.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    commandTwo.Parameters.AddWithValue("@DateLogged", current);

                    connection.Open();
                    command.ExecuteNonQuery();
                    commandTwo.ExecuteNonQuery();

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

        public IList<Hours> GetAllHours(int userId, bool all)
        {
            IList<Hours> defaultHoursList = new List<Hours>();
            IList<Hours> specificHoursList = new List<Hours>();

            if (all == true)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand(@"SELECT userID, taskId, timeInHours, dateLogged, description, location, isSubmitted,isApproved FROM Hours
                                                    WHERE userID = @userId;", connection);

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
                                                    WHERE userID = @userId
                                                    AND dateLogged BETWEEN CONVERT(datetime, @lastMonth) AND CONVERT(datetime, @currentDays);", connection);

                    command.Parameters.AddWithValue("@userid", userId);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastMonth", lastMonth);


                    SqlDataReader reader = command.ExecuteReader();

                    specificHoursList = MapHoursToReader(reader);
                }
                return specificHoursList;
            }
        }

        public Hours PullLoggedHours(int? userId)
        {
            throw new NotImplementedException();
        }

        public IList<Hours> GetTimeReport(int userid, string duration)
        {

            IList<Hours> payrollLog = new List<Hours>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                if (duration == "1W")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateLogged, Hours.description, Hours.location FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateLogged BETWEEN CONVERT(datetime, @lastMonth) AND CONVERT(datetime, @currentDays);", connection);
                    command.Parameters.AddWithValue("@userid", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastMonth", lastMonth);
                    SqlDataReader reader = command.ExecuteReader();

                    payrollLog = MapHoursToReader(reader);
                }
                else if (duration == "1M")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateLogged, Hours.description, Hours.location FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateLogged BETWEEN CONVERT(datetime, @lastWeek) AND CONVERT(datetime, @currentDays);", connection);
                    command.Parameters.AddWithValue("@userid", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastWeek", lastWeek);
                    SqlDataReader reader = command.ExecuteReader();

                    payrollLog = MapHoursToReader(reader);
                }
                else if (duration == "1Q")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateLogged, Hours.description, Hours.location FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateLogged BETWEEN CONVERT(datetime, @lastQuarter) AND CONVERT(datetime, @currentDays);", connection);
                    command.Parameters.AddWithValue("@userid", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastQuarter", lastQuarter);
                    SqlDataReader reader = command.ExecuteReader();

                    payrollLog = MapHoursToReader(reader);
                }
            }

            return payrollLog;
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
                    TimeInHours = Convert.ToDecimal(reader["timeInHours"]),
                    Date = Convert.ToDateTime(reader["dateLogged"]),
                    Description = Convert.ToString(reader["description"]),
                    Location = Convert.ToString(reader["location"]),
                };

                hours.Add(hour);
            }
            return hours;
        }
    }
}
