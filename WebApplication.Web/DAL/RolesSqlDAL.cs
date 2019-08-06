using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public bool CreateNewRole(Roles role)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO RollTable (Id, description) VALUES(@id, @description);", connection);


                    command.Parameters.AddWithValue("@Id", role.RoleId);
                    command.Parameters.AddWithValue("@description", role.Description);

                    command.ExecuteNonQuery();

                    if (role.RoleId == null || role.Description == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SqlException E)
            {
                Console.Write(E);
                throw;
            }

        }
    }
}
