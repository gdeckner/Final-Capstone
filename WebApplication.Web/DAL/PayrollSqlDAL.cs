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

                    SqlCommand command = new SqlCommand(@"INSERT INTO PayrollTable (UserId, StartDate, EndDate, IsApproved) VALUES(@UserId, @startdate, @enddate, @isapproved);", connection);


                    command.Parameters.AddWithValue("@UserId", report.UserId);
                    command.Parameters.AddWithValue("@startdate", report.StartDate);
                    command.Parameters.AddWithValue("@enddate", report.EndDate);
                    command.Parameters.AddWithValue("@isapproved", report.Approved);


                    command.ExecuteNonQuery();

                    if (report.UserId == null || report.StartDate == null || report.EndDate == null || report.Approved == false)
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
    }
}
