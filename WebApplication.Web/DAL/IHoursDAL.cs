﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IHoursDAL
    {
        bool CreateNewHours(Hours hour);

        Hours PullLoggedHours(int? userId);

        IList<Hours> GetAllHours(int userId);
    }
}
