using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class LogSqlDAL : ILogDAL
    {
        private readonly string connectionString;
        private readonly DateTime current = DateTime.Today;
        private readonly DateTime lastMonth = DateTime.Now.AddDays(-30);
        private readonly DateTime lastWeek = DateTime.Now.AddDays(-7);
        private readonly DateTime lastQuarter = DateTime.Now.AddDays(-120);

        public LogSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public IList<Log> GetUserLog(int userid, string duration)
        {

            IList<Log> userChangesLog = new List<Log>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                if (duration == "1W")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Log.targetUser, Log.dateWorked, Log.dateLogged, Log.modified_Date, Log.hoursId, Log.hoursBefore, Log.hoursAfter FROM Log                                                 
                                                    WHERE targetUser = @userId
                                                    AND Log.dateWorked BETWEEN CONVERT(datetime, @lastWeek) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY Log.dateWorked DESC;", connection);

                    command.Parameters.AddWithValue("@userId", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastWeek", lastWeek);
                    SqlDataReader reader = command.ExecuteReader();

                    userChangesLog = MapHoursToReader(reader);
                }
                else if (duration == "1M")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Log.targetUser, Log.dateWorked, Log.dateLogged, Log.modified_Date, Log.hoursId, Log.hoursBefore, Log.hoursAfter FROM Log                                                   
                                                    WHERE targetUser = @userId
                                                    AND Log.dateWorked BETWEEN CONVERT(datetime, @lastMonth) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY Log.dateWorked DESC;", connection);

                    command.Parameters.AddWithValue("@userId", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastMonth", lastMonth);
                    SqlDataReader reader = command.ExecuteReader();

                    userChangesLog = MapHoursToReader(reader);
                }
                else if (duration == "1Q")
                {
                    SqlCommand command = new SqlCommand(@"SELECT Log.targetUser, Log.dateWorked, Log.dateLogged, Log.modified_Date, Log.hoursId, Log.hoursBefore, Log.hoursAfter FROM Log                                                
                                                    WHERE targetUser = @userId
                                                    AND Log.dateWorked BETWEEN CONVERT(datetime, @lastQuarter) AND CONVERT(datetime, @currentDays)
                                                    ORDER BY Log.dateWorked DESC;", connection);
                    command.Parameters.AddWithValue("@userId", userid);
                    command.Parameters.AddWithValue("@currentDays", current);
                    command.Parameters.AddWithValue("@lastQuarter", lastQuarter);
                    SqlDataReader reader = command.ExecuteReader();

                    userChangesLog = MapHoursToReader(reader);
                }
            }

            return userChangesLog;
        }

        public int GetUserLogWithinPayPeriod(int userid, DateTime startPayPeriodDate, DateTime endPayPeriodDate)
        {

            int numOfUserLogsInPayPeriod = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                {
                    SqlCommand command = new SqlCommand(@"SELECT COUNT(dateWorked) 
                                                          FROM Hours
                                                          WHERE userID = @userId AND dateWorked BETWEEN @startDate AND @endDate;", connection);

                    command.Parameters.AddWithValue("@userId", userid);
                    command.Parameters.AddWithValue("@startDate", startPayPeriodDate);
                    command.Parameters.AddWithValue("@endDate", endPayPeriodDate);
                    numOfUserLogsInPayPeriod = (int)command.ExecuteScalar();

                }
            }

            return numOfUserLogsInPayPeriod;
        }

        private List<Log> MapHoursToReader(SqlDataReader reader)
        {
            List<Log> logs = new List<Log>();

            while (reader.Read())
            {
                Log log = new Log
                {
                    TargetUser = Convert.ToInt32(reader["targetUser"]),
                    DateWorked = Convert.ToDateTime(reader["dateWorked"]),
                    DateLogged = Convert.ToDateTime(reader["dateLogged"]),
                    ModifiedDate = Convert.ToDateTime(reader["modified_Date"]),
                    HoursId = Convert.ToInt32(reader["hoursId"]),
                    HoursBefore = Convert.ToDecimal(reader["hoursBefore"]),
                    HoursAfter = Convert.ToDecimal(reader["hoursAfter"]),
                };


                logs.Add(log);
            }
            return logs;
        }
    }
}
