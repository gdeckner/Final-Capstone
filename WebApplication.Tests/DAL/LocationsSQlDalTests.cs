using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class LocationsSQlDalTests : TimeClockDataBaseTest
    {
        private LocationSqlDAL dao;
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new LocationSqlDAL(ConnectionString);
        }
        [TestMethod]
        public void CreateLocationTests()
        {
            Location testLocation = new Location
            {
                Title = "Tech Elevator Space",
                Description = "Near the back of the building is where our team is located"
            };

            dao.CreateLocation(testLocation);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select location_Title,location_Description
                                    from Locations
                                    where location_Title = 'Tech Elevator Space'";
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    testLocation.Description = (string)reader["location_Description"];
                    testLocation.Title = (string)reader["location_Title"];
                }
            }

            Assert.AreEqual("Tech Elevator Space", testLocation.Title);
            Assert.AreEqual("Near the back of the building is where our team is located", testLocation.Description);
            Assert.AreNotEqual("Turtles", testLocation.Title);
        }
    }
}
