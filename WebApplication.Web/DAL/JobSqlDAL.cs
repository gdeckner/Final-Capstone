using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class JobSqlDAL : IJobDAL
    {
        private readonly string connectionString;

        public JobSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Job CreateTask(Job job)
        {
            Job placeholder = new Job();
            return placeholder;
        }


    }
}
