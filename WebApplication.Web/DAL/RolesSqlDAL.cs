using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class RolesSqlDAL : IRolesDAL
    {
        private readonly string connectionString;

        public RolesSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Roles CreateRole(Roles role)
        {
            Roles placeholder = new Roles();
            return placeholder;
        }
    }
}
