using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class LocationSqlDAL
    {
        private readonly string connectionString;

        public LocationSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public bool CreateLocation(Models.Location location)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(@"INSERT INTO Locations (location_Title, location_Description) VALUES(@title, @description);", connection);


                    command.Parameters.AddWithValue("@title", location.Title);
                    command.Parameters.AddWithValue("@description", location.Description);



                    command.ExecuteNonQuery();

                    if (location.Title == null || location.Description == null)
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
