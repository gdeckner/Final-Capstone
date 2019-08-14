using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class PayrollSqlDAL : IPayrollDAL
    {
        private readonly string connectionString;

        public PayrollSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string updatePayroll;

        public bool CreatePayReport(PayrollTable report)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO PayrollTable (UserId, StartDate, EndDate, IsApproved,isSubmitted) VALUES(@UserId, @startdate, @enddate, @isapproved,@isSubmitted);", connection);

                    command.Parameters.AddWithValue("@UserId", report.UserId);
                    command.Parameters.AddWithValue("@startdate", report.StartDate);
                    command.Parameters.AddWithValue("@enddate", report.EndDate);
                    command.Parameters.AddWithValue("@isapproved", report.IsApproved);
                    command.Parameters.AddWithValue("@isSubmitted", report.IsSubmitted);


                    command.ExecuteNonQuery();

                    if (report.UserId == null || report.StartDate == null || report.EndDate == null || report.IsApproved == false)
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

        public bool DeleteReport(PayrollTable report)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM Payroll WHERE payroll_Id = @Id AND startDate = @startDate AND endDate = @endDate;", connection);

                    cmd.Parameters.AddWithValue("@Id", report.UserId);
                    cmd.Parameters.AddWithValue("@startDate", report.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", report.EndDate);

                    cmd.ExecuteNonQuery();

                    if (report.UserId == null)
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

        public IList<PayrollTable> GetTimeReport(int userid)
        {

            IList<PayrollTable> payrollLog = new List<PayrollTable>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT user_Id, startDate, endDate, isSub FROM payroll
                                                    WHERE user_Id = @userId;", connection);
                command.Parameters.AddWithValue("@userid", userid);
                SqlDataReader reader = command.ExecuteReader();

                payrollLog = MapPayrollReader(reader);
            }
           

            return payrollLog;
        }

        private List<PayrollTable> MapPayrollReader(SqlDataReader reader)
        {
            List<PayrollTable> reports = new List<PayrollTable>();

            while (reader.Read())
            {
                PayrollTable report = new PayrollTable
                {
                    UserId = Convert.ToInt32(reader["userId"]),
                    StartDate = Convert.ToDateTime(reader["startDate"]),
                    EndDate = Convert.ToDateTime(reader["endDate"]),
                    IsApproved = Convert.ToBoolean(reader["isApproved"]),
                    IsSubmitted = Convert.ToBoolean(reader["isSubmitted"]),
                    Name = Convert.ToString(reader["first_Last_Name"])
                };

                reports.Add(report);
            }
            return reports;
        }

        public IList<PayrollTable> GetListOfTimeCards(DateTime startDate)
        {

            IList<PayrollTable> payrollLog = new List<PayrollTable>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT payroll.userId, payroll.startDate, payroll.endDate, payroll.isSubmitted, payroll.isApproved, userLogin.first_Last_Name FROM payroll
                                                    INNER JOIN userLogin
                                                    ON payroll.userId = userLogin.userId
                                                    WHERE payroll.startDate = @startDate;", connection);
                command.Parameters.AddWithValue("@startDate", startDate);
                SqlDataReader reader = command.ExecuteReader();

                payrollLog = MapPayrollReader(reader);
            }

            return payrollLog;
        }

        public IList<PayrollTable> GetListOfPayPeriods()
        {

            IList<PayrollTable> payrollLog = new List<PayrollTable>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT DISTINCT startDate, endDate FROM payroll", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PayrollTable report = new PayrollTable
                    {
                        StartDate = Convert.ToDateTime(reader["startDate"]),
                        EndDate = Convert.ToDateTime(reader["endDate"]),
                    };

                    payrollLog.Add(report);
                }
            }
            
            return payrollLog;
        }

        public void CreatePayPeriod(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Payroll (UserId, StartDate, EndDate, IsApproved, isSubmitted)
                                                          SELECT userID, @startdate, @enddate, 'False', 'False'
                                                          FROM UserLogin;", connection);

                    command.Parameters.AddWithValue("@startdate", StartDate);
                    command.Parameters.AddWithValue("@enddate", EndDate);

                    command.ExecuteNonQuery();

                }
            }
            catch (SqlException E)
            {
                Console.Write(E);
                throw;
            }
        }

        public bool AlertIfDaysNotSubmitted(int? userId)
        {
            bool needsAlert = false;
            int result = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();

                    cmd.CommandText = @"select userId,startDate,endDate,isApproved,isSubmitted from Payroll
                    where userId = @userId and endDate < Current_TimeStamp and isSubmitted = 0";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                if (result != 0)
                {
                    needsAlert = true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                
            }
            return needsAlert;
        }

        public bool ApproveTime(PayrollTable pay)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (pay.UserId != null && pay.StartDate != null && pay.EndDate != null && pay.IsApproved != false)
                    {
                        updatePayroll = @"(UPDATE Payroll
                                               SET isApproved = @IsApproved
                                               WHERE 
                                               userId = @UserId
                                               AND
                                               startDate = @StartDate
                                               AND 
                                               endDate = @EndDate
                                               AND isSubmitted != 0);";

                    }

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updatePayroll;


                    command.Parameters.AddWithValue("@IsApproved", pay.IsApproved);
                    command.Parameters.AddWithValue("@UserId", pay.UserId);
                    command.Parameters.AddWithValue("@StartDate", pay.StartDate);
                    command.Parameters.AddWithValue("@EndDate", pay.EndDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                if (pay.UserId == null)
                {
                    return false;
                }
                else
                {
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
