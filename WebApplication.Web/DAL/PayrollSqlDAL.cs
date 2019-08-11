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

                    SqlCommand cmd = new SqlCommand("DELETE FROM Tasks WHERE payroll_Id = @Id;", connection);

                    cmd.Parameters.AddWithValue("@Id", report.UserId);

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
                SqlCommand command = new SqlCommand(@"SELECT payroll.user_Id, payroll.startDate, payroll.endDate, payroll.isSub FROM payroll
                                                    INNER JOIN userJob
                                                    ON payroll.user_Id = hours.user_Id
                                                    WHERE payroll.userid = @userId;", connection);
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
                };

                reports.Add(report);
            }
            return reports;
        }
    }
}
