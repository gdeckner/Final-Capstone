using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class TasksSqlDAL : ITaskDAL
    {
        private readonly string connectionString;

        public TasksSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Tasks CreateTask(Task task)
        {
            Tasks placeholder = new Tasks();
            return placeholder;
        }



    }
}
