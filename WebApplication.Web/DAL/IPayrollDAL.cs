using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IPayrollDAL
    {
        bool CheckPayrollForDates(DateTime StartDate);

        bool CreatePayReport(PayrollTable report);

        IList<PayrollTable> GetListOfTimeCards(DateTime startDate);

        IList<PayrollTable> GetListOfPayPeriods();

        void CreatePayPeriod(DateTime startDate, DateTime endDate);

        bool ApproveTime(PayrollTable payrollLine);

        IList<PayrollTable> GetTimeReport(int userid);

        bool SubmitTime(PayrollTable payrollLine);
    }
}
