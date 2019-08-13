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
        private string beginHoursSession = @"(BEGIN TRANSACTION);";

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

                    SqlCommand command = new SqlCommand(@"INSERT INTO Hours (userID, taskID, timeInHours, dateWorked, dateLogged, description, location) VALUES(@UserId, @TaskId, @TimeInHours, @WorkedDate, @LoggedDate, @Description, @Location);", connection);
                    SqlCommand commandTitle = new SqlCommand(@"UPDATE Hours SET Hours.task_Title = (SELECT Tasks.project_Task_Title FROM Tasks WHERE Hours.taskId = Tasks.project_Task_ID) WHERE Hours.taskId = @TaskId;", connection);

                    command.Parameters.AddWithValue("@UserId", hour.UserId);
                    command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    command.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    command.Parameters.AddWithValue("@Description", hour.Description);
                    command.Parameters.AddWithValue("@Location", hour.Location);
                    command.Parameters.AddWithValue("@WorkedDate", hour.DateWorked);
                    command.Parameters.AddWithValue("@LoggedDate", current);
                    commandTitle.Parameters.AddWithValue("@TaskId", hour.TaskId);


                    command.ExecuteNonQuery();
                    commandTitle.ExecuteNonQuery();

                    if (hour.UserId == null || hour.TaskId == null || hour.TimeInHours == null || hour.DateWorked == null)
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
                    if (hour.TaskId != null && hour.OldTask != null)
                    {
                        string updateTask = @"(UPDATE H
                                               SET H.taskId = @TaskId
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE H.userID = @UserId
                                               AND H.taskId = @OldTaskId
                                               AND P.isApproved != '1');";
                        beginHoursSession += updateTask;
                    }
                    if (hour.Description != null)
                    {
                        string updateDescription = @"(UPDATE H
                                               SET H.Description = @Description
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE H.userID = @UserId
                                               AND H.taskId = @TaskId
                                               AND P.isApproved != '1');";

                        beginHoursSession += updateDescription;
                    }
                    if (hour.Location != null)
                    {
                        string updateLocation = @"(UPDATE H
                                               SET H.location = @Location
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE H.userID = @UserId
                                               AND H.taskId = @TaskId
                                               AND P.isApproved != '1');";

                        beginHoursSession += updateLocation;

                    }
                    if (hour.TimeInHours != null)
                    {
                        string updateHours = @"(UPDATE H
                                               SET H.timeInHours = @TimeInHours
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE H.userID = @UserId
                                               AND H.taskId = @TaskId
                                               AND P.isApproved != '1');";

                        beginHoursSession += updateHours;

                    }
                    if (hour.DateWorked != null)
                    {
                        string updateDateWorked = @"(UPDATE H
                                               SET H.dateWorked = @DateWorked
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE H.userID = @UserId
                                               AND H.taskId = @TaskId
                                               AND P.isApproved != '1');";

                        beginHoursSession += updateDateWorked;
                    }

                    string loggedDate = @"(UPDATE Hours SET dateLogged = @DateLogged WHERE userID = @UserId AND taskId = @TaskId AND isSubmitted != 1);";
                    string commit = @"(COMMIT);";
                    beginHoursSession += loggedDate;
                    beginHoursSession += commit;


                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = beginHoursSession;
                    if (hour.UserId != null)
                    {
                        command.Parameters.AddWithValue("@UserId", hour.UserId);
                    }
                    if (hour.OldTask != null)
                    {
                        command.Parameters.AddWithValue("@OldTask", hour.OldTask);
                    }
                    if (hour.TaskId != null)
                    {
                        command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    }
                    if (hour.Description != null)
                    {
                        command.Parameters.AddWithValue("@Description", hour.Description);
                    }
                    if (hour.Location != null)
                    {
                        command.Parameters.AddWithValue("@Location", hour.Location);
                    }
                    if (hour.TimeInHours != null)
                    {
                        command.Parameters.AddWithValue("@TaskId", hour.TimeInHours);
                    }
                    if (hour.DateWorked != null)
                    {
                        command.Parameters.AddWithValue("@TimeInHours", hour.DateWorked);
                    }


                    command.Parameters.AddWithValue("@DateLogged", current);

                    connection.Open();
                    command.ExecuteNonQuery();


                    if (hour.UserId == null)
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
                    SqlCommand command = new SqlCommand(@"SELECT userID, taskId, timeInHours, dateWorked, description, location, task_Title FROM Hours
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
                    SqlCommand command = new SqlCommand(@"SELECT userID, taskId, timeInHours, dateWorked, description, location, task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateWorked BETWEEN CONVERT(datetime, @lastMonth) AND CONVERT(datetime, @currentDays);", connection);

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
                    SqlCommand command = new SqlCommand(@"SELECT Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateWorked, Hours.description, Hours.location, Hours.task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateWorked BETWEEN CONVERT(datetime, @lastWeek) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY dateWorked DESC;", connection);
                    command.Parameters.AddWithValue("@userid", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastWeek", lastWeek);
                    SqlDataReader reader = command.ExecuteReader();

                    payrollLog = MapHoursToReader(reader);
                }
                else if (duration == "1M")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateWorked, Hours.description, Hours.location, Hours.task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateWorked BETWEEN CONVERT(datetime, @lastMonth) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY dateWorked DESC;", connection);
                    command.Parameters.AddWithValue("@userid", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastMonth", lastMonth);
                    SqlDataReader reader = command.ExecuteReader();

                    payrollLog = MapHoursToReader(reader);
                }
                else if (duration == "1Q")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateWorked, Hours.description, Hours.location, Hours.task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateWorked BETWEEN CONVERT(datetime, @lastQuarter) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY dateWorked DESC;", connection);
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
                    DateWorked = Convert.ToDateTime(reader["dateWorked"]),
                    Description = Convert.ToString(reader["description"]),
                    Location = Convert.ToString(reader["location"]),
                    TaskTitle = Convert.ToString(reader["task_Title"]),
                };

                hours.Add(hour);
            }
            return hours;
        }
    }
}
