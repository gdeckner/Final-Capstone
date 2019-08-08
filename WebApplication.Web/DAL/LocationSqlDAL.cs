using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class LocationSqlDAL : ILocationDAL
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

        public bool DeleteLocation(Models.Location location)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM Locations WHERE location_Title = @title;", connection);

                    cmd.Parameters.AddWithValue("@title", location.Title);

                    cmd.ExecuteNonQuery();

                    if (location.Title == null)
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

        public IList<Models.Location> GetAllLocations()
        {
            IList<Models.Location> locations = new List<Models.Location>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(@"SELECT location_Id, location_Title, location_Description FROM Locations", connection);
                SqlDataReader reader = command.ExecuteReader();

                locations = MapLocationReader(reader);
            }
            return locations;
        }

        private List<Models.Location> MapLocationReader(SqlDataReader reader)
        {
            List<Models.Location> locations = new List<Models.Location>();

            while (reader.Read())
            {
                Models.Location location = new Models.Location
                {
                    Title = Convert.ToString(reader["location_Title"]),
                    Description = Convert.ToString(reader["location_Description"]),
                };

                locations.Add(location);
            }
            return locations;
        }
    }
}
