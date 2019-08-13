using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IHoursDAL
    {
        bool CreateNewHours(Hours hour);

        //IList<Hours> GetAllHours(int userId);

        IList<Hours> GetTimeReport(int userid, string duration);

        IList<Hours> GetAllHours(int userId, bool all);

        bool UpdateHours(Hours hour);

        IList<Hours> GetTimeCard(int? userId, DateTime startDate, DateTime endDate);
    }
}
