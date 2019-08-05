using System;
using System.Collections.Generic;
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

        public PayrollTable CreatePayReport(PayrollTable report)
        {
            PayrollTable placeholder = new PayrollTable();
            return placeholder;
        }

    }
}
