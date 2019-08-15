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
        private string beginHoursSession = @"BEGIN TRANSACTION;";
        private readonly decimal before = 0;


        public HoursSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool CreateNewHours(Hours hour)
        {
            int result = AllowHoursIfPayPeriodExists(hour.DateWorked);

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    if (result > 0)
                    {
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
                    }

                    if (result <= 0)
                    {
                        SqlCommand commandLog = new SqlCommand(@"INSERT INTO Log (targetUser, dateWorked, dateLogged, modified_Date, hoursId, hoursBefore, hoursAfter, currentUser) 
VALUES(@UserId, @WorkedDate, @LoggedDate, @LoggedDate, (SELECT Hours.hoursId FROM Hours WHERE dateWorked = @WorkedDate AND Hours.userID = @UserId), @Before, @TimeInHours, @UserId);", connection);
                        commandLog.Parameters.AddWithValue("@UserId", hour.UserId);
                        commandLog.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                        commandLog.Parameters.AddWithValue("@Description", hour.Description);
                        commandLog.Parameters.AddWithValue("@Location", hour.Location);
                        commandLog.Parameters.AddWithValue("@WorkedDate", hour.DateWorked);
                        commandLog.Parameters.AddWithValue("@LoggedDate", current);
                        commandLog.Parameters.AddWithValue("@LoggedDateT", current);
                        commandLog.Parameters.AddWithValue("@Before", before);
                        commandLog.ExecuteNonQuery();
                    }

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





        public int AllowHoursIfPayPeriodExists(DateTime WorkDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();

                    cmd.CommandText = @"SELECT userId, startDate, endDate
                                        FROM Payroll
                                        WHERE startDate <= @WorkDate
                                        AND
                                        endDate >= @WorkDate";

                    cmd.Parameters.AddWithValue("@WorkDate", WorkDate);


                    var rowCount = cmd.ExecuteScalar();
                    return rowCount == null ? 0 : Convert.ToInt32(rowCount);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;

        }

        public bool UpdateHours(Hours hour)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //if (hour.TaskId != null && hour.OldTask != null)
                    //{
                    //    string updateTask = @"UPDATE H
                    //                           SET H.taskId = @TaskId
                    //                           FROM dbo.Hours as H
                    //                           INNER JOIN dbo.Payroll AS P
                    //                           ON H.userID = P.userId
                    //                           WHERE H.userID = @UserId
                    //                           AND H.taskId = @OldTaskId
                    //                           AND P.isApproved != 1;";

                    //    beginHoursSession += updateTask;
                    //}
                    if (hour.Description != null)
                    {
                        string updateDescription = @"UPDATE H
                                               SET H.Description = @Description
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE P.isApproved != 1
                                               AND H.hoursId = @HoursId;";

                        beginHoursSession += updateDescription;
                    }
                    if (hour.Location != null)
                    {
                        string updateLocation = @"UPDATE H
                                               SET H.location = @Location
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE P.isApproved != 1
                                               AND H.hoursId = @HoursId;";

                        beginHoursSession += updateLocation;

                    }
                    if (hour.TimeInHours != null)
                    {
                        string updateHours = @"UPDATE H
                                               SET H.timeInHours = @TimeInHours
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE P.isApproved != 1
                                               AND H.hoursId = @HoursId;";

                        string logUpdateHours = @"INSERT INTO Log (targetUser, dateWorked, dateLogged, modified_Date, hoursId, hoursBefore, hoursAfter, 
                                                currentUser) VALUES(@UserId, @WorkedDate, @LoggedDate, @LoggedDate, (SELECT hoursId FROM Hours 
                                                WHERE dateWorked = @WorkedDate AND userID = @UserId), (SELECT Log.hoursBefore FROM Log 
                                                WHERE dateWorked = @WorkedDate AND targetUser = @UserId), @TimeInHours, @UserId);";

                        beginHoursSession += updateHours;
                        beginHoursSession += logUpdateHours;


                    }
                    if (hour.DateWorked != null)
                    {
                        string updateDateWorked = @"UPDATE H
                                               SET H.dateWorked = @DateWorked
                                               FROM dbo.Hours as H
                                               INNER JOIN dbo.Payroll AS P
                                               ON H.userID = P.userId
                                               WHERE P.isApproved != 1
                                               AND H.hoursId = @HoursId;";

                        beginHoursSession += updateDateWorked;
                    }

                    string loggedDate = @"UPDATE Hours SET dateLogged = @DateLogged WHERE hoursId = @HoursId;";
                    string commit = @"COMMIT;";

                    beginHoursSession += loggedDate;
                    beginHoursSession += commit;


                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = beginHoursSession;
                    command.Parameters.AddWithValue("@UserId", hour.UserId);
                    //command.Parameters.AddWithValue("@OldTask", hour.OldTask);
                    command.Parameters.AddWithValue("@TaskId", hour.TaskId);
                    command.Parameters.AddWithValue("@Description", hour.Description);
                    command.Parameters.AddWithValue("@Location", hour.Location);
                    command.Parameters.AddWithValue("@TimeInHours", hour.TimeInHours);
                    command.Parameters.AddWithValue("@WorkedDate", hour.DateWorked);
                    command.Parameters.AddWithValue("@DateWorked", hour.DateWorked);
                    command.Parameters.AddWithValue("@LoggedDate", current);
                    command.Parameters.AddWithValue("@DateLogged", current);
                    command.Parameters.AddWithValue("@LoggedDateT", current);
                    command.Parameters.AddWithValue("@Before", before);
                    command.Parameters.AddWithValue("@HoursId", hour.HoursId);



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
                    SqlCommand command = new SqlCommand(@"SELECT hoursId, userID, taskId, timeInHours, dateWorked, description, location, task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    ORDER BY dateWorked DESC;", connection);

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
                    SqlCommand command = new SqlCommand(@"SELECT hoursId, userID, taskId, timeInHours, dateWorked, description, location, task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateWorked BETWEEN CONVERT(datetime, @lastMonth) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY dateWorked DESC;", connection);

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
                    SqlCommand command = new SqlCommand(@"SELECT Hours.hoursId, Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateWorked, Hours.description, Hours.location, Hours.task_Title FROM Hours
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
                    SqlCommand command = new SqlCommand(@"SELECT Hours.hoursId, Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateWorked, Hours.description, Hours.location, Hours.task_Title FROM Hours
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
                    SqlCommand command = new SqlCommand(@"SELECT Hours.hoursId, Hours.userID, Hours.taskId, Hours.timeInHours, Hours.dateWorked, Hours.description, Hours.location, Hours.task_Title FROM Hours
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

        public Hours GetHoursById(int hoursId)
        {
            Hours selectedHour = new Hours();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT hoursId, userID, taskId, timeInHours, dateWorked, description, location, task_Title FROM Hours
                                                WHERE hoursId = @hoursId
                                                ORDER BY dateWorked DESC;", connection);

                command.Parameters.AddWithValue("@hoursId", hoursId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Hours hour = new Hours
                    {
                        HoursId = Convert.ToInt32(reader["hoursId"]),
                        UserId = Convert.ToInt32(reader["userID"]),
                        TaskId = Convert.ToInt32(reader["taskId"]),
                        TimeInHours = Convert.ToDecimal(reader["timeInHours"]),
                        DateWorked = Convert.ToDateTime(reader["dateWorked"]),
                        Description = Convert.ToString(reader["description"]),
                        Location = Convert.ToString(reader["location"]),
                        TaskTitle = Convert.ToString(reader["task_Title"])
                    };

                    selectedHour = hour;
                }
            }

            return selectedHour;
        }

        private List<Hours> MapHoursToReader(SqlDataReader reader)
        {
            List<Hours> hours = new List<Hours>();

            while (reader.Read())
            {
                Hours hour = new Hours
                {
                    HoursId = Convert.ToInt32(reader["hoursId"]),
                    UserId = Convert.ToInt32(reader["userID"]),
                    TaskId = Convert.ToInt32(reader["taskId"]),
                    TimeInHours = Convert.ToDecimal(reader["timeInHours"]),
                    DateWorked = Convert.ToDateTime(reader["dateWorked"]),
                    Description = Convert.ToString(reader["description"]),
                    Location = Convert.ToString(reader["location"]),
                    TaskTitle = Convert.ToString(reader["task_Title"])
                };

                hours.Add(hour);
            }
            return hours;
        }

        public IList<Hours> GetTimeCard(int? userId, DateTime startDate, DateTime endDate)
        {
            IList<Hours> timeCard = new List<Hours>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT hoursId, userID, taskId, timeInHours, dateLogged, dateWorked, description, location, task_Title FROM Hours
                                                    WHERE userID = @userId
                                                    AND dateWorked BETWEEN CONVERT(datetime, @startDate) AND CONVERT(datetime, @endDate);", connection);

                command.Parameters.AddWithValue("@userid", userId);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);


                SqlDataReader reader = command.ExecuteReader();

                timeCard = MapHoursToReader(reader);
            }
            return timeCard;
        }

        public int IsOverWeeklyHoursAlert(int? userId, DateTime startDateOne, DateTime endDateTwo)
        {
            bool weekOneAlert = false;
            bool weekTwoAlert = false;
            int weeklyAlert = 0;
            decimal pulledSumHoursOne = 0;
            decimal pulledSumHoursTwo = 0;
            string userRole = "";
            DateTime endDateOne = startDateOne.AddDays(7);
            DateTime startDateTwo = endDateOne;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand(@"select SUM(timeInHours)from hours
                                                        where userID = @userId
                                                        AND dateWorked BETWEEN CONVERT(datetime, @startDateOne) AND CONVERT(datetime, @endDateOne);", connection);

                    SqlCommand commandTwo = new SqlCommand(@"select SUM(timeInHours)from hours
                                                        where userID = @userId
                                                        AND dateWorked BETWEEN CONVERT(datetime, @startDateTwo) AND CONVERT(datetime, @endDateTwo);", connection);

                    command.Parameters.AddWithValue("@userid", userId);
                    command.Parameters.AddWithValue("@startDateOne", startDateOne);
                    command.Parameters.AddWithValue("@endDateOne", endDateOne);
                    command.Parameters.AddWithValue("@startDateTwo", startDateTwo);
                    command.Parameters.AddWithValue("@endDateTwo", endDateTwo);


                    pulledSumHoursOne = Convert.ToDecimal(command.ExecuteScalar());
                    pulledSumHoursTwo = Convert.ToDecimal(command.ExecuteScalar());

                    command.CommandText = @"select userRole from userlogin where userId = @userIdName";
                    command.Parameters.AddWithValue("@userIdName", userId);

                    userRole = Convert.ToString(command.ExecuteScalar());



                    if (userRole == "User FT" && pulledSumHoursTwo > 40)
                    {
                        weekTwoAlert = true;
                    }
                    if (userRole == "User PT" && pulledSumHoursTwo > 27.5M)
                    {
                        weekTwoAlert = true;
                    }
                    if (userRole == "User FT" && pulledSumHoursOne > 40 && pulledSumHoursTwo == 0)
                    {
                        weekOneAlert = true;
                    }
                    if (userRole == "User PT" && pulledSumHoursOne > 27.5M && pulledSumHoursTwo == 0)
                    {
                        weekOneAlert = true;
                    }
                }
                if (weekTwoAlert == true)
                {
                    weeklyAlert = 2;
                    return weeklyAlert;
                }
                else if (weekOneAlert == true)
                {
                    weeklyAlert = 1;
                    return weeklyAlert;
                }

                return weeklyAlert;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
