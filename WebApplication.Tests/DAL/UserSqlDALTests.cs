using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Security;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class UserSqlDALTests : TimeClockDataBaseTest
    {
        private UserSqlDAL dao;
        [TestInitialize]
        public override void Setup()
        {

            base.Setup();
            PasswordHasher hash = new PasswordHasher();
            dao = new UserSqlDAL(ConnectionString, new PasswordHasher());
            string salt = Convert.ToBase64String(hash.GenerateRandomSalt());
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"insert into Roles (roles_Title,role_Description) values ('Admin','Admin control' ),('Users','Generic User')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"insert into UserLogin (first_Last_Name,userName,userRole,password,salt) values ('Gerg DinkleBerry','gdeckner','Admin',@password,@salt)";
                cmd.Parameters.AddWithValue("@salt", "RrQlUO2CbmowsGDSpRhXZA==");
                cmd.Parameters.AddWithValue("@password", "RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs");

                cmd.ExecuteNonQuery();

            }
        }
        [TestMethod]
        public void CheckIfUserNameExistsTest()
        {
            Assert.AreEqual(true, dao.CheckIfUserNameExists("GDECKNER"));
            Assert.AreEqual(true, dao.CheckIfUserNameExists("gdeckner"));
            Assert.AreEqual(false, dao.CheckIfUserNameExists("ooga"));

        }
        [TestMethod]
        public void PullUserRoleTest()
        {
            Assert.AreEqual("Admin", dao.PullUserRole("gdeckner"));
            Assert.AreNotEqual("User", dao.PullUserRole("gdeckner"));
        }
        [TestMethod]
        public void CheckLoginTest()
        {
            Assert.AreEqual(true, dao.CheckLogin("GDeckner", "Password"));
            Assert.AreEqual(false, dao.CheckLogin("turtle", "password"));
            Assert.AreEqual(true, dao.CheckLogin("GDeckner", "Password"));

        }
        [TestMethod]
        public void CreateUserTest()
        {

            User testUser = new User
            {
                Name = "Merkle Chowbuster",
                Password = "RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs",
                Salt = "RrQlUO2CbmowsGDSpRhXZA==",
                Role = "Users",
                Username = "MChowbuster"

            };

            dao.CreateUser(testUser);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from UserLogin where userName = 'MChowbuster'";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    testUser.Name = (string)reader["first_Last_Name"];
                    testUser.Role = (string)reader["userRole"];
                    testUser.Password = (string)reader["password"];
                }
            }

            Assert.AreEqual("Users", testUser.Role);

        }
        [TestMethod]
        public void DeleteUserTest()
        {

            int result;

            User testUser = new User();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from UserLogin where userName = 'gdeckner'";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    testUser.Name = (string)reader["first_Last_Name"];
                    testUser.Role = (string)reader["userRole"];
                    testUser.Password = (string)reader["password"];
                    testUser.UserId = (int)reader["userId"];
                    testUser.Username = (string)reader["userName"];
                }
            }


            dao.DeleteUser(testUser.UserId, testUser.UserId);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from UserLogin where userName = 'gdeckner'";
                result = Convert.ToInt32(cmd.ExecuteScalar());

            }

            Assert.AreNotEqual(0, result);

            dao.DeleteUser(testUser.UserId, 999999);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"select * from UserLogin where userName = 'gdeckner'";
                result = Convert.ToInt32(cmd.ExecuteScalar());

            }
            Assert.AreEqual(0, result);






        }
    }
}
