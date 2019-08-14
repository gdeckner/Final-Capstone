using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface ILogDAL
    {
        IList<Log> GetUserLog(int userid, string duration);

        int GetUserLogWithinPayPeriod(int userid, DateTime startPayPeriodDate, DateTime endPayPeriodDate);
    }
}
