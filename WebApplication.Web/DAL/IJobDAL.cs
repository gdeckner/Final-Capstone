﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IJobDAL
    {
        bool AssignUserToJob(UserJob assigned);

        bool CreateNewJob(Job job);

        List<Job> GetJobList();

        bool CreateNewTask(Tasks task);
    }
}
