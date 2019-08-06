using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WebApplication.Web.Security;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

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
                cmd.CommandText = @"insert into UserLogin (first_Last_Name,userName,userRole,password,salt) values ('Gerg DinkleBerry','gdeckner','Admin',@password,@salt)";
                cmd.Parameters.AddWithValue("@salt", "RrQlUO2CbmowsGDSpRhXZA==");
                cmd.Parameters.AddWithValue("@password", "RrQlUO2CbmowsGDSpRhXZPGjRy1BEXkN3fdCrNs4xUJjxNcs");
                cmd.ExecuteNonQuery();
            }
        }
        [TestMethod]
        public void CheckIfUserNameExistsTest()
        {
            Assert.AreEqual(true, dao.CheckIfUserNameExists("testUser"));
            Assert.AreEqual(true, dao.CheckIfUserNameExists("testuser"));
            Assert.AreEqual(false, dao.CheckIfUserNameExists("ooga"));

        }
        public void PullUserRoleTest()
        {
            Assert.AreEqual("Admin", dao.PullUserRole("gdeckner"));
            Assert.AreNotEqual("User", dao.PullUserRole("gdeckner"));
        }
        public void CreateUserTest()
        {
            User testUser = new User
            {
               Username = "NewUser",
               Password = 
            }
            dao.CreateUser()
        }
    }
}
