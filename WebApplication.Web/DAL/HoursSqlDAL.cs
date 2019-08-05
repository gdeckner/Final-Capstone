using System;
using System.Collections.Generic;
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

        public Hours CreateRole(Hours hour)
        {
            Hours placeholder = new Hours();
            return placeholder;
        }
    }
}
